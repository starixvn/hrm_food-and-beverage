namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum OrderedBy
    {
        [Description("All")]
        All = 0,

        [Description("Me")]
        Me = 1,

        [Description("Not Me")]
        NotMe = 2
    }
}
