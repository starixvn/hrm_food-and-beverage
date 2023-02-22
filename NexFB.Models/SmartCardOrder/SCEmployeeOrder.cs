namespace NexFB.Models.SmartCardOrder
{
    public class SCEmployeeOrder
    {
        public int OrderId { get; set; }

        public int QuantityOrder { get; set; }

        public int OrderNo { get; set; }

        public string MealEngName { get; set; }

        public string MealVnmName { get; set; }

        public bool IsMainMenu { get; set; }

        public bool IsSideDish { get; set; }

        public string OrderFullName { get; set; }

        public string OrderNickName { get; set; }

        public string OrderFrom { get; set; }

        public int CurrentStatusId { get; set; }
    }
}