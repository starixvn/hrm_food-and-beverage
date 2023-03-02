namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading;
    using System.Web.Http;
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

    public class OrderingController : BaseController
    {
        private readonly IOrderingService _orderService;
        private readonly ISideDishService _sideDishService;

        public OrderingController()
            : this(null, null)
        {
        }

        public OrderingController(
            IOrderingService orderService,
            ISideDishService sideDishService)
        {
            _orderService = orderService ?? new OrderingService();
            _sideDishService = sideDishService ?? new SideDishService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public IList<MealOrderingModel> GetMyOrder()
        {
            var criteria = new GetMyOrderInCurrentDayCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            IList<MealOrderingModel> orders = _orderService.GetMyOrderByCurrentDay(criteria);
            return orders;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public Menu GetMyMenu()
        {
            var criteria = new GetMenuByUserIdCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            Menu menu = _orderService.GetMenuByUserId(criteria);
            return menu;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public Menu GetMenuByUserId(int userId)
        {
            var criteria = new GetMenuByUserIdCriteria()
            {
                UserId = userId
            };

            Menu menu = _orderService.GetMenuByUserId(criteria);
            return menu;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetAddNew()
        {
            var criteria = new GetMemberNewOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberNewOrder(criteria);
            return order;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetSavedOrder()
        {
            var criteria = new GetMemberSavedOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberSavedOrder(criteria);
            return order;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetOrderNewOrder(int userId)
        {
            var criteria = new GetMemberNewOrderCriteria()
            {
                UserId = userId
            };

            MealOrderingModel order = _orderService.GetMemberNewOrder(criteria);
            return order;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public MealOrderingModel GetOrderById(int orderId)
        {
            var criteria = new GetOrderByIdCriteria()
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetOrderById(criteria);
            order.CurrentUser = new Employee()
            {
                UserId = this.CurrentUser.UserId,
                NickName = this.CurrentUser.NickName,
                FullName = this.CurrentUser.FullName
            };

            return order;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Read)]
        public IEnumerable<OrderRemarkModel> GetOrderRemark()
        {
            var criteria = new GetOrderRemarkCriteria()
            {
                RegisterBy = this.CurrentUser.UserId
            };

            IEnumerable<OrderRemarkModel> listRemark = _orderService.GetOrderRemark(criteria);

            return listRemark;
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public object InsertOrder(MealOrderingModel model)
        {
            var criteria = new InsertOrderCriteria()
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                IsSavedOrder = model.IsSavedOrder,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                RemarkVNM = model.IsTranslateRemark ? TranslateRemark(model.Remark) : model.Remark,
                Remark = model.Remark,
                Members = model.Members,
                TakeAway = model.TakeAway
            };

            CommandResult result = _orderService.InsertOrder(criteria);
            return new { result = result, orderId = criteria.OrderId };
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AdminOrderHistoryReport, Policy = Policy.Read)]
        public MealOrderingModel GetMemberNewOrderByAdmin(int userId, int mealId, DateTime onDate)
        {
            var criteria = new GetMemberNewOrderByStaffCriteria()
            {
                UserId = userId,
                MealId = mealId,
                OnDate = onDate
            };

            MealOrderingModel order = _orderService.GetMemberNewOrderByAdmin(criteria);
            return order;
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AdminOrderHistoryReport, Policy = Policy.Read)]
        public CommandResult AdminUpdateOrder(MealOrderingModel model)
        {
            var criteria = new AdminUpdateOrderCriteria()
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                OrderTime = DateTime.Now,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                OrderFrom = model.OrderFrom,
                MealID = model.MealID,
                StatusID = model.StatusId,
                Members = model.Members,
            };

            CommandResult result = _orderService.AdminUpdateOrder(criteria);
            return result;
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult UpdateOrder(MealOrderingModel model)
        {
            var criteria = new UpdateOrderCriteria()
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

            CommandResult result = _orderService.UpdateOrder(criteria);
            return result;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult Delete(int orderId)
        {
            var criteria = new DeleteOrderCriteria()
            {
                OrderId = orderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            CommandResult result = _orderService.DeleteOrder(criteria);
            return result;
        }

        public Employee GetCurrentUser()
        {
            var user = new Employee()
            {
                UserId = this.CurrentUser.UserId,
                NickName = this.CurrentUser.NickName,
                FullName = this.CurrentUser.FullName
            };

            return user;
        }

        public IEnumerable<Menu> GetAlternativeMenusByUserId(int userId)
        {
            var criteria = new GetAlternativeMenuByUserIdCriteria
            {
                UserId = userId
            };

            return _orderService.GetAlternativeMenuByUserId(criteria);
        }

        public IEnumerable<SideDishModel> GetSideDishByUserId(int userId)
        {
            var criteria = new GetSideDishByUserIdCriteria
            {
                UserId = userId
            };

            return _sideDishService.GetSideDishByUserId(criteria);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public object InsertOrderSideDish(MealOrderingModel model)
        {
            var criteria = new InsertOrderSideDishCriteria()
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                RemarkVNM = model.IsTranslateRemark ? TranslateRemark(model.Remark) : model.Remark,
                Remark = model.Remark,
                Members = model.Members,
                TakeAway = model.TakeAway
            };

            CommandResult result = _orderService.InsertOrderSideDish(criteria);
            return new { result = result, orderId = criteria.OrderId };
        }

        [HttpPut]
        public CommandResult UpdateOrderSideDish(MealOrderingModel model)
        {
            var criteria = new UpdateOrderSideDishCriteria()
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                Phone = model.Phone,
                RemarkVNM = model.IsTranslateRemark ? TranslateRemark(model.Remark) : model.Remark,
                Remark = model.Remark,
                Members = model.Members,
                TakeAway = model.TakeAway
            };

            CommandResult result = _orderService.UpdateOrderSideDish(criteria);
            return result;
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AdminOrderHistoryReport, Policy = Policy.Read)]
        public CommandResult AdminUpdateOrderSideDish(MealOrderingModel model)
        {
            var criteria = new AdminUpdateOrderSideDishCriteria()
            {
                OrderId = model.OrderId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                LocationId = (model.Location != null && model.Location.LocationId != 0) ? (int?)model.Location.LocationId : null,
                OrderTime = DateTime.Now,
                OrderFrom = model.OrderFrom,
                Members = model.Members,
                StatusID = model.StatusId,
            };

            CommandResult result = _orderService.AdminUpdateOrderSideDish(criteria);
            return result;
        }

        [HttpGet]
        public bool CheckValidModifiedTime(string startTime, int orderId)
        {
            var criteria = new GetLastMofifiedDateByOrderIdCriteria
            {
                OrderId = orderId
            };

            MealOrderingModel order = _orderService.GetLastModifiedDateByOrderId(criteria);
            DateTime startDateTime = DateTime.Parse(startTime);

            if (startDateTime > order.ModifiedDate)
            {
                return true;
            }

            return false;
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Order, Policy = Policy.Host)]
        public CommandResult ChangeMealOrder(UpdateMealOrderCriteria criteria)
        {
            criteria.ModifiedBy = this.CurrentUser.UserId;
            criteria.ModifiedDate = DateTime.Now;

            CommandResult result = _orderService.UpdateMealOrder(criteria);
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