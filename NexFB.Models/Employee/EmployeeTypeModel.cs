namespace NexFB.Models.Employee
{
    using System;

    /// <summary>
    /// The employee type model.
    /// </summary>
    [Serializable]
    public class EmployeeTypeModel
    {
        public int EmployeeTypeId { get; set; }

        public string EmployeeTypeName { get; set; }
    }
}
