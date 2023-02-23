namespace NexFB.Site.Controllers.API
{
    using System;
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
        public AdminOrderHistoryReport GetData(
            DateTime fromDate,
            DateTime toDate,
            int? userId = null,
            int employeeMenu = 0,
            int orderPlatform = 0,
            string mealId = "",
            string statusId = "",
            string companyId = "",
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            var criteria = new GetAdminOrderHistoryCriteria
            {
                UserId = userId,
                FromDate = fromDate,
                ToDate = toDate,
                OrderBy = 0,
                MyMenu = employeeMenu,
                OrderPlatform = orderPlatform,
                MealId = mealId,
                StatusId = statusId,
                CompanyId = companyId
            };

            AdminOrderHistoryReport fullReport;
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId);
            }
            else
            {
                fullReport = reportService.GetAdminOrderHistoryReport(criteria);
                SessionManager.Set<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId, fullReport);
            }

            var pagingReport = reportService.GetPagingReport(fullReport, startIndex, itemsPerPage);

            return pagingReport;
        }
    }
}