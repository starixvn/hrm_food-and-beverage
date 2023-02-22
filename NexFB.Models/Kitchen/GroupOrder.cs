namespace NexFB.Models.Kitchen
{
    public class GroupOrder
    {
        public int GroupOrderId { get; set; }

        public short CompanyId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int LocationId { get; set; }

        public string LocationEngName { get; set; }

        public string LocationVnmName { get; set; }

        public string FloorName { get; set; }
    }
}
