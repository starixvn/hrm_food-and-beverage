namespace NexFB.WorkerService.Middlewares;

public class LoggingRequestMiddleware(ILogger<LoggingRequestMiddleware> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Sending HTTP request: {Method} {Url}", request.Method, request.RequestUri);

        var response = await base.SendAsync(request, cancellationToken);

        logger.LogInformation("Received response: {StatusCode}", response.StatusCode);

        return response;
    }
}