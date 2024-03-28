namespace NexFB.WCFService.Translator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using NLangDetect.Core;

    public static class FBTranslator
    {
        public static string AutoTranslation(string sourceText)
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
                        translatedText = TranslateText(sourceText);
                    }
                }
            }
            catch (Exception)
            {
                return sourceText;
            }

            return translatedText;
        }

        private static string TranslateText(string sourceText)
        {
            string translatedText = "";

            try
            {
                string url = string.Format(
                    "https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=vi&dt=t&q={0}",
                    HttpUtility.UrlEncode(sourceText));

                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Mozilla/5.0");
                string result = webClient.DownloadString(url);

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