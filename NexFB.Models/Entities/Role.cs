namespace NexFB.Models.Entities
{
    using System.Collections.Generic;

    public class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public IList<Company> Companies { get; set; } = new List<Company>();
    }
}