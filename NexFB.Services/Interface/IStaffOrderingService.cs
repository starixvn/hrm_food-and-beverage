
namespace NexFB.Services.Interface
{
    using NexFB.Core.Models;
    using NexFB.Criteria.StaffOrdering;
    using NexFB.Models.Ordering;
    using System.Collections.Generic;

    public interface IStaffOrderingService
    {
        MealOrderingModel GetMemberNewOrderByStaff(GetMemberNewOrderByStaffCriteria criteria);
        IList<MealOrderingModel> GetStaffOrder(GetStaffOrdersCriteria criteria);
        CommandResult InsertOrder(InsertOrderByStaffCriteria criteria);
        CommandResult UpdateOrderByStaff(UpdateOrderByStaffCriteria criteria);
        CommandResult DeleteOrderByStaff(DeleteOrderByStaffCriteria criteria);
    }
}
