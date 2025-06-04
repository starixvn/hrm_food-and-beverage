namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum FoodMenuType
    {
        [Description("All")]
        All = 0,

        [Description("Has Main Dish")]
        HasMainDish = 3,

        [Description("Has Side Dish (Charged)")]
        HasSideDish = 4
    }
}
