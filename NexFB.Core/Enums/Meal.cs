namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum Meal
    {
        [Description("None")]
        None = 0,

        [Description("Breakfast")]
        Breakfast = 1,

        [Description("Lunch")]
        Lunch = 2,

        [Description("Dinner")]
        Dinner = 3,

        [Description("Supper")]
        Supper = 4
    }
}
