namespace NexFB.Models.Entities
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Section
    {
        public int SectionId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<Team> Teams { get; set; }
    }
}
