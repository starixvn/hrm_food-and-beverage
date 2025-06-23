namespace NexFB.Site.Controllers.API
{
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Http;
    using System.Xml;

    public class MealCancelRegisterController : ApiController
    {
        private readonly IMealRegistrationService _mealRegistrationService;

        public MealCancelRegisterController()
             : this(null)
        {
        }

        public MealCancelRegisterController(
                    IMealRegistrationService mealRegistrationService)
        {
            _mealRegistrationService = mealRegistrationService ?? new MealRegistrationService();
        }

        [HttpPost]
        public CommandResult CancelRegisterMealByDate(CancelRegisterMealByDateCriteria criteria)
        {
            criteria.ModifiedBy = criteria.SysId;
            criteria.ModifiedDate = DateTime.Now;

            return _mealRegistrationService.CancelRegisterMealByDate(criteria);
        }

        [HttpPost]
        public IList<EmployeeRegister> GetEmployeeRegisterByDate(GetEmployeeRegisterMealByDateCriteria criteria)
        {
            return _mealRegistrationService.GetEmployeeRegisterByDate(criteria);
        }

    }
}
