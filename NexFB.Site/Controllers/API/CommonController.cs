// ----------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/29/2014 11:19:47 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Collections.Generic;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Team;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Common controller.
    /// </summary>
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IDeliveryAreaService _deliveryAreaService;

        public CommonController()
            : this(null, null)
        {
        }

        public CommonController(ICommonService commonService, IDeliveryAreaService deliveryAreaService)
        {
            _commonService = commonService ?? new CommonService();
            _deliveryAreaService = deliveryAreaService ?? new DeliveryAreaService();
        }

        public IEnumerable<Department> GetAllDepartment()
        {
            var criteria = new GetAllDepartmentCriteria();
            return _commonService.GetAllDepartment(criteria);
        }

        public IEnumerable<Location> GetAllDeliveryArea(
            bool? isActive = true,
            int? deliveryAreaId = null,
            string query = "")
        {
            var criteria = new GetAllDeliveryAreaCriteria
            {
                DeliveryAreaId = deliveryAreaId,
                IsActive = isActive,
                Query = query
            };

            return _deliveryAreaService.GetAllDeliveryArea(criteria);
        }

        public IEnumerable<Team> GetAllTeamByCompany(int departmentId)
        {
            var criteria = new GetAllTeamByCompanyCriteria
            {
                DepartmentId = departmentId
            };

            return _commonService.GetAllTeamByCompany(criteria);
        }

        public IEnumerable<MealTimesheet> GetAllMealTimesheet()
        {
            var criteria = new GetMealTimesheetCriteria();
            return _commonService.GetAllMealTimesheet(criteria);
        }

        public IEnumerable<EmployeeTypeModel> GetAllEmployeeType(
            string departmentId = "",
            bool? isLocal = null)
        {
            var criteria = new GetAllEmployeeTypeCriteria
            {
                DepartmentId = departmentId,
                IsLocal = isLocal
            };

            return _commonService.GetAllEmployeeType(criteria);
        }
    }
}