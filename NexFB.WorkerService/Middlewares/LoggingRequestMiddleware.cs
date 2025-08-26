using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NexFB.WorkerService.Middlewares;

public class LoggingRequestMiddleware(Fanex.Logging.ILogger logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync(cancellationToken);
            }

            return response;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"Error processing request: {request.Method} {request.RequestUri} {ex.Message}", ex);
            throw;
        }
    }
}