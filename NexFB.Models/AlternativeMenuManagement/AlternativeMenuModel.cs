namespace NexFB.Models.AlternativeMenuManagement
{
    using System.Collections.Generic;
    using NexFB.Models.Employee;

    /// <summary>
    /// The alternative menu model.
    /// </summary>
    public class AlternativeMenuModel
    {
        public int AlternativeMenuId { get; set; }

        public string AlternativeMenuCode { get; set; }

        public string AlternativeMenuVnmName { get; set; }

        public string AlternativeMenuEngName { get; set; }

        public string AlternativeMenuPhoto { get; set; }

        public bool? IsActive { get; set; }

        public bool IsMainMenu { get; set; }

        public IList<EmployeeTypeModel> EmployeeTypes { get; set; }
    }
}
