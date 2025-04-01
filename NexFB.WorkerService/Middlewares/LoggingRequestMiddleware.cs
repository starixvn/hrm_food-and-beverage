using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NexFB.WorkerService.Middlewares;

public class LoggingRequestMiddleware(ILogger<LoggingRequestMiddleware> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending HTTP request: {Method} {Url}", request.Method, request.RequestUri);

        var response = await base.SendAsync(request, cancellationToken);
        var totalRecords = 0;
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<JArray>(content);
            totalRecords = result?.Count ??  0;
        }

        logger.LogInformation("Received response: {StatusCode}. Total records sync: {Records}", response.StatusCode, totalRecords);

        return response;
    }
}