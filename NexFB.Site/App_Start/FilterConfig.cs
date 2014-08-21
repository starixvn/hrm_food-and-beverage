// ----------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:30:14 PM">Created.</change>
// </history>

namespace Fanex.NexFB.Site
{
    using System.Web.Mvc;

    /// <summary>
    /// Filter Config.
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}