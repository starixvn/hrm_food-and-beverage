namespace NexFB.Models.Kitchen
{
    public class EmployeeOrderStatus
    {
        public int MealId { get; set; }

        public int OrderId { get; set; }

        public int MenuId { get; set; }

        public string MenuEngName { get; set; }

        public string MenuVnmName { get; set; }

        public bool IsAllowOrder { get; set; }
    }
}