namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum GroupByReport
    {
        [Description("Employee")]
        Employee = 0,

        [Description("Department")]
        Department = 1,

        [Description("CostCenter")]
        CostCenter = 2,
    }
}