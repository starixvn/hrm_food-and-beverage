namespace NexFB.Pos.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Kitchen Group Button.
    /// </summary>
    public enum KitchenGroupButton
    {
        /// <summary>
        /// Release All Button.
        /// </summary>
        [Description("ReleaseAllButton")]
        ReleaseAllButton = 0,

        /// <summary>
        /// Pick Room Button.
        /// </summary>
        [Description("PickRoomButton")]
        PickRoomButton = 1,

        /// <summary>
        /// Pick Group Button.
        /// </summary>
        [Description("PickGroupButton")]
        PickGroupButton = 2,

        /// <summary>
        /// Choose Main Food Button.
        /// </summary>
        [Description("ChooseMainFoodButton")]
        ChooseMainFoodButton = 3,

        /// <summary>
        /// Choose Alternative Food Button.
        /// </summary>
        [Description("ChooseAlternativeFoodButton")]
        ChooseAlternativeFoodButton = 4,
    }
}
