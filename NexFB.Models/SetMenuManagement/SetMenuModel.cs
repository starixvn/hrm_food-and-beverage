namespace NexFB.Models.SetMenuManagement
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// SetMenu Model.
    /// </summary>
    public class SetMenuModel
    {
        public SetMenuModel() 
        {
            RawFoodIds = new List<int>();
            RawMenuIds = new List<int>();
        }

        public int SetMenuId { get; set; }

        public string SetMenuCode { get; set; }

        public string SetMenuEngName { get; set; }

        public string SetMenuVnmName { get; set; }

        public bool IsActive { get; set; }

        public IList<FoodSetMenuModel> FoodItems { get; set; }

        public IList<MenuSetMenuModel> Menus { get; set; }

        public IList<int> RawFoodIds { get; set; }

        public IList<int> RawMenuIds { get; set; }

        public string FoodItemText
        {
            get
            {
                string names = FoodItems != null && FoodItems.Any()
                    ? string.Join(", ", FoodItems.Select(t => t.FoodVnmName))
                    : string.Empty;
                return names;
            }
        }

        public string MenuText
        {
            get
            {
                string names = Menus != null && Menus.Any()
                    ? string.Join(", ", Menus.Select(t => t.MenuVnmName))
                    : string.Empty;
                return names;
            }
        }
    }
}
