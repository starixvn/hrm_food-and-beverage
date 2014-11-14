// --------------------------------------------------------------------------
// <copyright file="FBTranslator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.12">Created</change>
// </history>

namespace NexFB.Core.Translator
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web;
    using DialogueMaster.Babel;
    using DialogueMaster.Classification;

    /// <summary>
    /// FB Translator.
    /// </summary>
    public static class FBTranslator
    {
        private static IBabelModel _languageModel = BabelModel._AllModel;

        /// <summary>
        /// Auto Translation text.
        /// </summary>
        /// <param name="sourceText">Source text.</param>
        /// <returns>Translated text into vietnamese language.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]        
        public static string AutoTranslation(string sourceText)
        {
            string languageName = "vi";
            string translatedText = string.Empty;
            try
            {
                sourceText = sourceText.ToLower();
                ICategoryList result = _languageModel.ClassifyText(sourceText);

                if (result != null)
                {
                    languageName = result[0].Name;

                    if (languageName != "vi")
                    {
                        translatedText = TranslateText(languageName, sourceText);
                    }
                    else
                    {
                        translatedText = sourceText;
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
        /// <param name="sourceLanguage">Translate from language.</param>
        /// <param name="sourceText">Source text.</param>
        /// <returns>Vietnamese language.</returns>
        private static string TranslateText(string sourceLanguage, string sourceText)
        {
            string url = string.Format(
                "http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}",
                HttpUtility.UrlEncode(sourceText),
                string.Format("{0}|vi", sourceLanguage));
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
