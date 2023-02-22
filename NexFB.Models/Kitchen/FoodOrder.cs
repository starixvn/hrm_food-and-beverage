namespace NexFB.Models.Kitchen
{
    public class FoodOrder
    {
        public int FoodId { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodCode { get; set; }

        public string FoodEngName { get; set; }

        public string FoodPhoto { get; set; }

        public bool IsActive { get; set; }

        public decimal Price { get; set; }
    }
}