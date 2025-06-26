namespace NexFB.Models.FoodItemManagement
{
    /// <summary>
    /// The raw food menu model.
    /// </summary>
    public class RawFoodSetMenuModel
    {
        public int FoodId { get; set; }

        public int SetMenuId { get; set; }

        public string SetMenuVnmName { get; set; }

        public string SetMenuEngName { get; set; }
    }
}
