namespace NexFB.Models.FoodMealPlanning
{
    using System;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Models.SetMenuManagement;

    /// <summary>
    /// The Meal Planning model.
    /// </summary>
    public class MealPlanningModel
    {
        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsExist { get; set; }

        public bool IsDisable { get; set; }

        public bool IsCurrentMeal { get; set; }

        public SetMenuModel SetMenu { get; set; }

        public FoodMenuModel Menu { get; set; }
    }
}
