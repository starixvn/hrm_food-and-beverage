namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading;
    using System.Web.Mvc;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.StaffOrdering;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;

    public class KitchenStaffController : BaseController
    {
        private readonly IOrderingService _orderService;
        private readonly IStaffOrderingService _staffOrderService;

        public KitchenStaffController()
           : this(null, null)
        {
        }

        public KitchenStaffController(
            IOrderingService orderService,
            IStaffOrderingService staffOrderService)
        {
            _orderService = orderService ?? new OrderingService();
            _staffOrderService = staffOrderService ?? new StaffOrderingService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.KitchenStaffOrder, Policy = Policy.Read)]
        public MealOrderingModel GetNewOrderByStaff(int userId, int mealId, DateTime onDate)
        {
            var criteria = new GetMemberNewOrderByStaffCriteria
            {
                UserId = userId,
                MealId = mealId,
                OnDate = onDate
            };

            MealOrderingModel order = _staffOrderService.GetMemberNewOrderByStaff(criteria);
            return order;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.KitchenStaffOrder, Policy = Policy.Read)]
        public IList<MealOrderingModel> GetStaffOrder(DateTime OnDate)
        {
            var criteria = new GetStaffOrdersCriteria
            {
                UserId = this.CurrentUser.UserId,
                OnDate = OnDate
            };

            IList<MealOrderingModel> orders = _staffOrderService.GetStaffOrder(criteria);
            return orders;
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.KitchenStaffOrder, Policy = Policy.Host)]
        public object InsertStaffOrder(MealOrderingModel model)
        {
            var criteria = new InsertOrderByStaffCriteria
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsSavedOrder = model.IsSavedOrder,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                RemarkVNM = model.IsTranslateRemark ? TranslateRemark(model.Remark) : model.Remark,
                Remark = model.Remark,
                Members = model.Members,
                TakeAway = model.TakeAway,
                StatusID = model.StatusId,
                MealID = model.MealID,
                OnDate = model.OnDate
            };

            CommandResult result = _staffOrderService.InsertOrder(criteria);
            return new { result, orderId = criteria.OrderId };
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.KitchenStaffOrder, Policy = Policy.Host)]
        public CommandResult UpdateStaffOrder(MealOrderingModel model)
        {
            var criteria = new UpdateOrderByStaffCriteria
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                IsSavedOrder = false,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                RemarkVNM = model.IsTranslateRemark ? TranslateRemark(model.Remark) : model.Remark,
                Remark = model.Remark,
                Members = model.Members,
                TakeAway = model.TakeAway
            };

            CommandResult result = _staffOrderService.UpdateOrderByStaff(criteria);
            return result;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.KitchenStaffOrder, Policy = Policy.Host)]
        public CommandResult Delete(int orderId)
        {
            var criteria = new DeleteOrderByStaffCriteria
            {
                OrderId = orderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            CommandResult result = _staffOrderService.DeleteOrderByStaff(criteria);
            return result;
        }

        private string TranslateRemark(string remark)
        {
            string translatedText = string.Empty;

            if (!string.IsNullOrEmpty(remark))
            {
                string translatorAPIUrl = ConfigurationManager.AppSettings["TranslatorAPIUrl"];
                Thread translatorThread = new Thread(
                    () =>
                    {
                        translatedText = ThreadTranslator(remark, translatorAPIUrl);
                    });

                translatorThread.Start();

                if (!translatorThread.Join(TimeSpan.FromSeconds(Constants.MaximumTimeTranslateRemark)))
                {
                    translatedText = remark;
                }
            }

            return translatedText;
        }

        private string ThreadTranslator(string remark, string translatorAPIUrl)
        {
            return _orderService.TranslateRemark(remark, translatorAPIUrl);
        }
    }
}