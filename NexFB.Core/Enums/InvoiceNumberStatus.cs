namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum InvoiceNumberStatus
    {
        [Description("None")]
        None = 0,

        [Description("Receive Order Food Invoice")]
        ReceiveOrderFoodInvoice = 1,

        [Description("Deliver Order Food Invoice")]
        DeliverOrderFoodInvoice = 2,
    }
}
