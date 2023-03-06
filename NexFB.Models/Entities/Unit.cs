namespace NexFB.Models.Entities
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Unit
    {
        public int UnitId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<Department> Departments { get; set; }
    }
}
