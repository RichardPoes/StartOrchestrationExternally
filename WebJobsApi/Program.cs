using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebJobsApi;

// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((hbc, services) =>
            {
                services.AddDurableClientFactory(options =>
                {
                    options.IsExternalClient = true;
                    options.ConnectionName = "AzureWebJobsStorage";
                    options.TaskHub = "DurableFunctionsHub";
                });
            })
            .Build();

        host.Run();
    }
}