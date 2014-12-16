// --------------------------------------------------------------------------
// <copyright file="FBTranslator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.17">Created</change>
// </history>

namespace NexFB.Core.Translator
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;
    using NLangDetect.Core;

    /// <summary>
    /// The translator.
    /// </summary>
    public static class FBTranslator
    {
        /// <summary>
        /// Auto Translation text.
        /// </summary>
        /// <param name="sourceText">Source text.</param>
        /// <returns>Translated text into vietnamese language.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        public static string AutoTranslation(string sourceText)
        {
            LanguageDetector.Initialize();
            string translatedText = sourceText;
            try
            {
                sourceText = sourceText.ToLower();
                List<Language> results = LanguageDetector.DetectLanguage(sourceText);

                if (results != null && results.Count > 0)
                {
                    bool isVietnamese = results.Any(x => x.Name.ToString().ToLower() == "vi");
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

        /// <summary>
        /// Google translate text.
        /// </summary>
        /// <param name="sourceText">Source text.</param>
        /// <returns>Vietnamese language.</returns>
        private static string TranslateText(string sourceText)
        {
            string url = string.Format(
                "http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}",
                HttpUtility.UrlEncode(sourceText),
                "auto|vi");
            WebClient webClient = new WebClient();
            ////webClient.Encoding = System.Text.Encoding.UTF8;
            string result = webClient.DownloadString(url);
            result = result.Substring(result.IndexOf("<span title=\"", StringComparison.CurrentCulture) + "<span title=\"".Length);
            result = result.Substring(result.IndexOf(">", StringComparison.CurrentCulture) + 1);
            result = result.Substring(0, result.IndexOf("</span>", StringComparison.CurrentCulture));

            return HttpUtility.HtmlDecode(result.Trim());
        }
    }
}
