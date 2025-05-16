using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NexFB.WorkerService.Middlewares;

public class LoggingRequestMiddleware(Fanex.Logging.ILogger logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            var totalRecords = 0;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<JArray>(content);
                totalRecords = result?.Count ?? 0;
            }

            return response;
        }
        catch (Exception ex)
        {
            logger.Error($"Error processing request: {request.Method} {request.RequestUri} {ex.Message}", ex);
            throw;
        }

    }
}