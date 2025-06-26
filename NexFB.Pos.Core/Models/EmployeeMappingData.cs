namespace NexFB.Pos.Core.Models
{
    using System.Collections.Generic;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Employee Mapping Data.
    /// </summary>
    public class EmployeeMappingData
    {
        public int SysId { get; set; }

        public List<OrderMenu> ListOrderDetails { get; set; }
    }
}
