namespace NexFB.Models.FoodMenuManagement
{
    using System.Collections.Generic;
    using NexFB.Models.Employee;
    using NexFB.Models.MealRegistration;
    using NexFB.Models.SetMenuManagement;

    /// <summary>
    /// The food menu model.
    /// </summary>
    public class FoodMenuModel
    {
        public FoodMenuModel() 
        {
            SetMenus = new List<SetMenuModel>();
            EmployeeTypes = new List<EmployeeTypeModel>();
            Meals = new List<MealModel>();
        }

        public int MenuId { get; set; }

        public string MenuVnmName { get; set; }

        public string MenuEngName { get; set; }

        public string MenuEngMaker { get; set; }

        public bool? IsVegetarian { get; set; }

        public string Description { get; set; }

        public double PricePerMeal { get; set; }

        public double PricePerDay { get; set; }

        public bool? IsActive { get; set; }

        public int NumberFoodItems { get; set; }

        public IList<SetMenuModel> SetMenus { get; set; }

        public IList<EmployeeTypeModel> EmployeeTypes { get; set; }

        public IList<MealModel> Meals { get; set; }
    }
}
