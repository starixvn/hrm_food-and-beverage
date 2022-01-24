namespace NexFB.WebService.Translator
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
            string url = string.Format(
                "http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}",
                HttpUtility.UrlEncode(sourceText),
                "auto|vi");
            using (var webClient = new WebClient())
            {
                ////webClient.Encoding = System.Text.Encoding.UTF8;
                string result = webClient.DownloadString(url);
                result = result.Substring(result.IndexOf("<span title=\"", StringComparison.CurrentCulture) + "<span title=\"".Length);
                result = result.Substring(result.IndexOf(">", StringComparison.CurrentCulture) + 1);
                result = result.Substring(0, result.IndexOf("</span>", StringComparison.CurrentCulture));

                return HttpUtility.HtmlDecode(result.Trim());
            }
        }
    }
}