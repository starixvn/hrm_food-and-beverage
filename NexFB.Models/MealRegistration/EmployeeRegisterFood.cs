using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Models.MealRegistration
{
    public class EmployeeRegisterFood
    {
        public DateTime WorkDate { get; set; }
        public int MealId { get; set; }
        public bool? IsVegetarian { get; set; }
        public bool? IsVegetarianManual { get; set; }
    }
}
