namespace NexFB.Pos.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Kitchen Screen.
    /// </summary>
    public enum KitchenScreen
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Waiting Food Screen.
        /// </summary>
        [Description("WaitingFoodScreen")]
        WaitingFoodScreen = 1,

        /// <summary>
        /// Awaiting Deliver Food Screen.
        /// </summary>
        [Description("AwaitingDeliverFoodScreen")]
        AwaitingDeliverFoodScreen = 2,

        /// <summary>
        /// Delivered Food Screen.
        /// </summary>
        [Description("DeliveredFoodScreen")]
        DeliveredFoodScreen = 3,

        /// <summary>
        /// Canceled Food Screen.
        /// </summary>
        [Description("CanceledFoodScreen")]
        CanceledFoodScreen = 4,
    }
}
