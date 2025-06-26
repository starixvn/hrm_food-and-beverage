namespace NexFB.Models.Authority
{
    using System.Collections.Generic;

    /// <summary>
    /// The Menu Group.
    /// </summary>
    public class MenuGroup : MenuItem
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
    }
}
