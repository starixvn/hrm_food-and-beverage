namespace NexFB.Models.Entities
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Department
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<Section> Sections { get; set; }
    }
}
