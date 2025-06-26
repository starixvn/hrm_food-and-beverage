namespace NexFB.Web.Handlers
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Encoding Delegate Handler.
    /// </summary>
    public class EncodingDelegateHandler : DelegatingHandler
    {
        /// <summary>
        /// Override send request.
        /// </summary>
        /// <param name="request">Http Request Message.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Http Response Message.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(
                (responseToCompleteTask) => 
            {
                HttpResponseMessage response = responseToCompleteTask.Result;

                if (response.RequestMessage.Headers.AcceptEncoding != null &&
                    response.RequestMessage.Headers.AcceptEncoding.Count > 0)
                {
                    string encodingType = response.RequestMessage.Headers.AcceptEncoding.First().Value;

                    response.Content = new CompressedContent(response.Content, encodingType);
                }

                return response;
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
