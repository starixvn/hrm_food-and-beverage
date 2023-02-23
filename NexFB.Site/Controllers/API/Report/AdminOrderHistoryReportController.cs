namespace NexFB.Site.Controllers.API
{
    using System.Web.Mvc;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.AdminOrderHistoryReport;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface.Report;
    using NexFB.Web;
    using NexFB.Web.Attributes;

    public class AdminOrderHistoryReportController : BaseController
    {
        private readonly IAdminOrderHistoryService reportService;

        public AdminOrderHistoryReportController() : this(null)
        {
        }

        public AdminOrderHistoryReportController(IAdminOrderHistoryService reportService)
        {
            this.reportService = reportService ?? new AdminOrderHistoryService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AdminOrderHistoryReport, Policy = Policy.Read)]
        public AdminOrderHistoryReport GetData([System.Web.Http.FromUri] GetAdminOrderHistoryCriteria criteria)
        {
            AdminOrderHistoryReport fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId);
            }
            else
            {
                fullReport = reportService.GetAdminOrderHistoryReport(criteria);
                SessionManager.Set<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId, fullReport);
            }

            var pagingReport = reportService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);

            return pagingReport;
        }
    }
}