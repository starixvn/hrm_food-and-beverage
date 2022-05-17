using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Models.MealRegistration
{
    public class EmployeeRegisterModel
    {
        public DateTime WorkDate { get; set; }
        public int SysId { get; set; }
        public int MealID { get; set; }
        public bool? IsVegetarian { get; set; }
    }
}
