
namespace NexFB.Services.Interface
{
    using NexFB.Core.Models;
    using NexFB.Criteria.StaffOrdering;
    using NexFB.Models.Ordering;

    public interface IStaffOrderingService
    {
        MealOrderingModel GetMemberNewOrderByStaff(GetMemberNewOrderByStaffCriteria criteria);
        CommandResult InsertOrder(InsertOrderByStaffCriteria criteria);
    }
}
