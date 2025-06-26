namespace NexFB.Models.Authority
{
    /// <summary>
    /// The Menu Item.
    /// </summary>
    public class MenuItem
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string MenuLink { get; set; }

        public int? ParentId { get; set; }

        public int FunctionId { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsDisabled { get; set; }
    }
}
