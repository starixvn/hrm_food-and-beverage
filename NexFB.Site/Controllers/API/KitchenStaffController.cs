// <history>
//   <change who="Teddy.le" date="2021.3.5">Created</change>
// </history>
namespace NexFB.Site.Controllers.API
{
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Ordering;
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Criteria.StaffOrdering;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Models.SideDishManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using Menu = Models.Entities.Menu;

    public class KitchenStaffController : BaseController
    {
        private readonly IOrderingService _orderService;
        private readonly IStaffOrderingService _staffOrderService;
        private readonly ISideDishService _sideDishService;

        public KitchenStaffController()
           : this(null, null, null)
        {
        }

        public KitchenStaffController(
            IOrderingService orderService,
            IStaffOrderingService staffOrderService,
            ISideDishService sideDishService)
        {
            _orderService = orderService ?? new OrderingService();
            _staffOrderService = staffOrderService ?? new StaffOrderingService();
            _sideDishService = sideDishService ?? new SideDishService();
        }

        /// <summary>
        /// Get order to new order.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The model meal ordering.</returns>
        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.KitchenStaffOrder, Policy = Policy.Read)]
        public MealOrderingModel GetOrderNewOrderByStaff(int userId, int mealId, DateTime onDate)
        {
            var criteria = new GetMemberNewOrderByStaffCriteria()
            {
                UserId = userId,
                MealId = mealId,
                OnDate = onDate
            };

            MealOrderingModel order = _staffOrderService.GetMemberNewOrderByStaff(criteria);
            return order;
        }


        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public object InsertStaffOrder(MealOrderingModel model)
        {
            var criteria = new InsertOrderByStaffCriteria()
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
                StatusID = model.StatusId
            };

            CommandResult result = _staffOrderService.InsertOrder(criteria);
            return new { result = result, orderId = criteria.OrderId };
        }

        /// <summary>
        /// Translate remark into vietnamese.
        /// </summary>
        /// <param name="remark">The remark of order.</param>
        /// <returns>The translated text.</returns>
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
