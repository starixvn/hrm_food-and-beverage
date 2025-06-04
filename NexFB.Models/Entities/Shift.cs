namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// The Shift model.
    /// </summary>
    [Serializable]
    public class Shift
    {
        public int ShiftId { get; set; }

        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }
    }
}
