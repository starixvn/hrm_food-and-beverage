namespace NexFB.Models.SideDishManagement
{
    using System.Collections.Generic;
    using NexFB.Models.Employee;

    /// <summary>
    /// The side dish model.
    /// </summary>
    public class SideDishModel
    {
        public SideDishModel()
        {
            EmployeeTypes = new List<EmployeeTypeModel>();
        }

        public int FoodId { get; set; }

        public string FoodCode { get; set; }

        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string Name
        {
            get
            {
                return FoodEngName;
            }
        }

        public string FoodPhoto { get; set; }

        public double Price { get; set; }

        public bool IsActive { get; set; }

        public bool KitchenOnly { get; set; }

        public UnitModel Unit { get; set; }

        public IList<EmployeeTypeModel> EmployeeTypes { get; set; }
    }
}
