namespace NexFB.Models.Authority
{
    using System;

    /// <summary>
    /// User Function Model.
    /// </summary>
    [Serializable]
    public class UserFunction
    {
        public int FunctionId { get; set; }

        public string FunctionKey { get; set; }

        public bool IsRead { get; set; }

        public bool IsHost { get; set; }
    }
}
