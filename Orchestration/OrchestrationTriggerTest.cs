using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;

namespace Orchestration;

public class OrchestrationTriggerTest
{
    [Function(nameof(MyActivity))]
    public static async Task<string> MyActivity([ActivityTrigger] string input)
    {
        return "It worked";
    }

    [Function("MyOrchestration")]
    public static async Task<string> MyOrchestration([OrchestrationTrigger] TaskOrchestrationContext context,
        string input)
    {
        // implementation
        return await context.CallActivityAsync<string>(nameof(MyActivity), input);
    }

    [Function("HttpTrigger1")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        await client.ScheduleNewOrchestrationInstanceAsync("MyOrchestration");
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }
}