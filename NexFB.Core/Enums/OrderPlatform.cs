namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum OrderPlatform
    {
        [Description("All")]
        All = 0,

        [Description("Web")]
        Web = 1,

        [Description("POS - Kitchen")]
        PosKitchen = 2,

        [Description("POS - Employee")]
        PosEmployee = 3
    }
}
