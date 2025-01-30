using RenewalGovernancePremiumValidation.BussinessLogic;
using RenewalGovernancePremiumValidation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Hosting;
using RenewalGovernancePremiumValidation;
using Serilog.Filters;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog.Events;
using RenewalGovernancePremiumValidation.Models.Domain;
using DocumentFormat.OpenXml.InkML;
using System.Diagnostics;
using System.Collections;

var builder = Host.CreateDefaultBuilder(args).ConfigureLogging((context, logging) =>
{
    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
});
string logFilePath = @"C:\temp\RenewalGovernancePremiumValidationSchedular\RenewalGovernancePremiumValidationSchedular_log.txt";
Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
Log.Information("Application has started.");
Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
   .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Hour,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
      .Filter.ByExcluding(logEvent =>
        logEvent.Properties.ContainsKey("SourceContext") &&
        logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command") &&
        logEvent.Level == Serilog.Events.LogEventLevel.Information &&
        logEvent.MessageTemplate.Text.Contains("Executed DbCommand")  // Exclude logs that contain 'Executed DbCommand'
    ).CreateLogger();
string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"]?.ConnectionString;
if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Connection string is missing from app.config");
    return;
}
builder.ConfigureServices((context, services) =>
{
    services.AddLogging(configure => configure.AddSerilog());
    services.AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString));
    services.AddTransient<KotaIPA>();
    services.AddTransient<OptimaSenior>();
    services.AddTransient<OptimaVital>();
    services.AddTransient<AarogyaSanjeevani>();
    services.AddHostedService<MyWorker>();
});
var host = builder.Build();
builder.ConfigureServices((context, services) =>
{
    services.AddLogging(configure => configure.AddConsole());
    services.AddHostedService<MyWorker>();
    services.AddTransient<Program>();
    services.AddSingleton<KotaIPA>();
    services.AddSingleton<OptimaSenior>();
    services.AddSingleton<OptimaVital>();
    services.AddSingleton<AarogyaSanjeevani>();
});
Console.WriteLine("Schedular is Started!");
Console.WriteLine("Renewal Governance Premium Validation Schedular Started!");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var serviceProviderOR = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<KotaIPA>()
    .BuildServiceProvider();
var serviceProviderOS = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<OptimaSenior>()
    .BuildServiceProvider();
var serviceProviderOV = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<OptimaVital>()
    .BuildServiceProvider();
var serviceProviderAR = new ServiceCollection().AddLogging(logging => logging.AddSerilog())
    .AddDbContext<HDFCDbContext>(options =>
        options.UseNpgsql(connectionString))
    .AddTransient<AarogyaSanjeevani>()
    .BuildServiceProvider();
var kotaipa = serviceProviderOR.GetService<KotaIPA>();
var optimaSenior = serviceProviderOS.GetService<OptimaSenior>();
var optimaVital = serviceProviderOV.GetService<OptimaVital>();
var aarogyaSanjeevani = serviceProviderAR.GetService<AarogyaSanjeevani>();
string postgresConnectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
using (var postgresConnection = new NpgsqlConnection(postgresConnectionString))
{
    try
    {
        postgresConnection.Open();
        try
        {
            List<string> idPlaceholders = new List<string>();
            var listofpolicies = optimaSenior.FetchNewBatchIds(postgresConnection);
            Console.WriteLine(listofpolicies.Count);
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<HDFCDbContext>();
                var baserates = await optimaSenior.GetRatesAsync(dbContext);
                var relations = await aarogyaSanjeevani.GetRelationTagsAsync(dbContext);
                var ovbaserates = await optimaVital.GetRatesAsync(dbContext);
                var aasrates = await aarogyaSanjeevani.GetBaseRatesAsync(dbContext);

                if (listofpolicies.Count > 0)
                {
                    var tasks = new List<System.Threading.Tasks.Task>();
                    {
                        var semaphore = new SemaphoreSlim(10);

                        foreach (var item in listofpolicies)
                        {
                            var task = System.Threading.Tasks.Task.Run(async () =>
                            {
                                await semaphore.WaitAsync();
                                try
                                {
                                    string certificateNo = item[0];
                                    string productCode = item[1];
                                    switch (productCode)
                                    {
                                        case "2804":
                                            await optimaSenior.GetOptimaSeniorValidation(certificateNo, baserates);
                                            break;

                                        case "2861":
                                            var osRNEDataAarogya = await aarogyaSanjeevani.GetAarogyaSanjeevaniValidation(certificateNo, aasrates, relations);
                                            break;

                                        case "2813":
                                            var osRNEDataVital = await optimaVital.GetOptimaVitalValidation(certificateNo, ovbaserates);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                finally
                                {
                                    semaphore.Release();  
                                }
                            });
                            tasks.Add(task);
                        }
                        await System.Threading.Tasks.Task.WhenAll(tasks);
                    }
                }
            }
        }

        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while processing calculating premium.");
            Console.WriteLine("Error occurred: " + ex.Message);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while processing the application.");
        Console.WriteLine("Error occurred: " + ex.Message);
    }


    Console.WriteLine("Schedular is Completed!");
    Log.Information("Application has finished processing.");
    Log.CloseAndFlush();
}