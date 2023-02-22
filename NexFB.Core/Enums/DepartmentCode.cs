namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum DepartmentCode
    {
        [Description("None")]
        None,

        [Description("Nexcel")]
        NX,

        [Description("SMS")]
        Sms,

        [Description("HM")]
        HM,

        [Description("Luminet")]
        Luminet,
    }
}