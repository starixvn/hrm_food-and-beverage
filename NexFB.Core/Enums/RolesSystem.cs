namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum RolesSystem
    {
        [Description("None")]
        None = 0,

        [Description("SA")]
        SA = 1,

        Finance = 2,

        [Description("Kitchen")]
        Kitchen = 3,

        [Description("Employee")]
        Employee = 4,

        [Description("Admin")]
        Admin = 5
    }
}