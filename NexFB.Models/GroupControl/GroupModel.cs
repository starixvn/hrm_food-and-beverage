namespace NexFB.Models.GroupControl
{
    /// <summary>
    /// Group model.
    /// </summary>
    public class GroupModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int Members { get; set; }
    }
}
