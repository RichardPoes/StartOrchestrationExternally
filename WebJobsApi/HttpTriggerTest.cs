using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.ContextImplementations;
using Microsoft.Azure.WebJobs.Extensions.DurableTask.Options;

namespace WebJobsApi;

public class HttpTriggerTest(IDurableClientFactory factory)
{
    [Function("HttpTrigger1")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var client = factory.CreateClient(new DurableClientOptions
        {
            IsExternalClient = true,
            ConnectionName = "AzureWebJobsStorage",
            TaskHub = "DurableFunctionsHub"
        });
        await client.StartNewAsync("MyOrchestration");
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}