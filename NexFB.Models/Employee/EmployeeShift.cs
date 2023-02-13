using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexFB.Models.Employee
{
    [Serializable]
    public class EmployeeShift
    {
        public int SysID { get; set; }
        public DateTime WorkDate { get; set; }
        public string Shift { get; set; }
    }
}
