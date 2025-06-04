namespace NexFB.Web.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using NexFB.Resources;

    /// <summary>
    /// Compressed Content.
    /// </summary>
    public class CompressedContent : HttpContent
    {
        private HttpContent _originalContent;
        private string _encodingType;

        public CompressedContent(HttpContent content, string encodingType)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType");
            }

            _originalContent = content;
            this._encodingType = encodingType.ToLower();

            if (this._encodingType != "gzip" && this._encodingType != "deflate")
            {
                throw new InvalidOperationException(string.Format(Lang.EncodingNotSupported, this._encodingType));
            }

            // copy the headers from the original content
            foreach (KeyValuePair<string, IEnumerable<string>> header in _originalContent.Headers)
            {
                this.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            this.Headers.ContentEncoding.Add(encodingType);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;

            return false;
        }

        /// <summary>
        /// Function to Serialize to stream.
        /// </summary>
        /// <param name="stream">The Stream.</param>
        /// <param name="context">Transport Context.</param>
        /// <returns>Result the Task.</returns>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;

            if (_encodingType == "gzip")
            {
                compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
            }
            else if (_encodingType == "deflate")
            {
                compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);
            }

            return _originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }
    }
}
