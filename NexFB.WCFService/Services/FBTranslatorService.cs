namespace NexFB.WCFService.Services
{
    using System.Collections.Generic;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using NLangDetect.Core;
    using System.Linq;
    using System.Web;

    public interface IFBTranslatorService
    {
        Task<string> AutoTranslation(string sourceText);
    }

    public class FBTranslatorService(IHttpClientFactory httpClientFactory) : IFBTranslatorService
    {
        public async Task<string> AutoTranslation(string sourceText)
        {
            LanguageDetector.Initialize();
            string translatedText = sourceText;
            try
            {
                sourceText = sourceText.ToLowerInvariant();
                List<Language> results = LanguageDetector.DetectLanguage(sourceText);

                if (results != null && results.Count > 0)
                {
                    bool isVietnamese = results.Any(x => x.Name.ToString().ToLowerInvariant() == "vi");
                    if (!isVietnamese)
                    {
                        translatedText = await TranslateText(sourceText);
                    }
                }
            }
            catch (Exception)
            {
                return sourceText;
            }

            return translatedText;
        }

        private async Task<string> TranslateText(string sourceText)
        {
            string translatedText = "";

            try
            {
                string url = string.Format(
                    "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=vi&dt=t&q={0}",
                    HttpUtility.UrlEncode(sourceText));

                var httpClient = httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                string result = await httpClient.GetStringAsync(url);

                int startIndex = result.IndexOf("[[[\"") + 4;
                int endIndex = result.IndexOf("\",\"", startIndex);
                translatedText = result.Substring(startIndex, endIndex - startIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error translating text: " + ex.Message);
            }

            return translatedText;
        }
    }
}
