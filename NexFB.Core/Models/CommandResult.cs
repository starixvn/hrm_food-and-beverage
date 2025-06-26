namespace NexFB.Core.Models
{
    using NexFB.Resources;

    public class CommandResult
    {
        public CommandResult()
        {
            Code = 1;
            Message = Lang.InvalidInputData;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return Code == 0;
            }
        }
    }
}
