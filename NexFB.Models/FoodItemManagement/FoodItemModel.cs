namespace NexFB.Models.FoodItemManagement
{
    using System.Collections.Generic;
    using NexFB.Models.SetMenuManagement;

    /// <summary>
    /// The food item model.
    /// </summary>
    public class FoodItemModel
    {
        public FoodItemModel()
        {
            SetMenus = new List<SetMenuModel>();
        }

        public int FoodId { get; set; }

        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodCode { get; set; }

        public string FoodPhoto { get; set; }

        public bool? IsActive { get; set; }

        public IList<SetMenuModel> SetMenus { get; set; }
    }
}
