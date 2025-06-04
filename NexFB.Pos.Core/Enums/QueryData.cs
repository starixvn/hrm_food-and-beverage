namespace NexFB.Pos.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Query Data.
    /// </summary>
    public enum QueryData
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Get New Order.
        /// </summary>
        [Description("GetNewOrder")]
        GetNewOrder = 1,

        /// <summary>
        /// Get All Data.
        /// </summary>
        [Description("GetAllData")]
        GetAllData = 2,
    }
}
