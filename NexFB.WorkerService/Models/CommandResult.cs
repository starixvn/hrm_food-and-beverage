namespace NexFB.WorkerService.Models
{
    public class CommandResult
    {
        public int Code { get; set; } = 1;
        public string Message { get; set; } = "Invalid input data";

        public short CompanyId { get; set; }

        public bool IsSuccessful => Code == 0;

        public long OutputId { get; set; }
    }
}
