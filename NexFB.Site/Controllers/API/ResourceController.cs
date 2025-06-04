namespace NexFB.Site.Controllers.API
{
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using NexFB.Resources;

    /// <summary>
    /// Resource controller.
    /// </summary>
    public class ResourceController : BaseController
    {
        /// <summary>
        /// Function to get resource by language.
        /// </summary>
        /// <param name="lang">Language input.</param>
        /// <returns>String resource JSON format.</returns>
        public string Get()
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Lang)) { IgnoreCase = true };
            ResourceSet resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            var dictionary = resourceSet.Cast<DictionaryEntry>();
            var items = dictionary.Select(dict => string.Format("\"{0}\" : \"{1}\"", dict.Key, string.Join(",", dict.Value)));
            var jsonString = "{" + string.Join(",", items) + "}";

            return jsonString;
        }
    }
}
