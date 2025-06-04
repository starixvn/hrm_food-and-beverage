namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// The Team entity.
    /// </summary>
    [Serializable]
    public class Team
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
