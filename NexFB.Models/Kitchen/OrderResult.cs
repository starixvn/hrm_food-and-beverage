namespace NexFB.Models.Kitchen
{
    using NexFB.Core.Models;

    public class OrderResult
    {
        public CommandResult CommandResult { get; set; }

        public int OrderId { get; set; }

        public int OrderNo { get; set; }
    }
}