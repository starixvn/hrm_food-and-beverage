using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexFB.Models.SmartCardOrder
{
    public class SCMainDishEmployeeOrder
    {
        public int SysId { get; set; }
        public string NickName { get; set; }
        public bool IsFreeOrder { get; set; }
        public bool? IsRegistration { get; set; }
    }
}
