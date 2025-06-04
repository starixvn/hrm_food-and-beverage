namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum OrderStatus
    {
        [Description("None")]
        None = 0,

        [Description("New")]
        NewOrder = 1,

        [Description("Accepted")]
        ReceivedOrder = 2,

        [Description("Reduced")]
        ReducedOrder = 3,

        [Description("Delivered")]
        DeliveredOrder = 4,

        [Description("Edited")]
        EditedOrder = 5,

        [Description("Rejected")]
        CanceledOrder = 6,

        [Description("Deleted")]
        DeletedOrder = 99,
    }
}
