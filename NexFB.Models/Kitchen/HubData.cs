namespace NexFB.Models.Kitchen
{
    using System;

    /// <summary>
    /// Class HubData.
    /// </summary>
    [Serializable]
    public class HubData
    {
        public int CurrentOrderId { get; set; }

        public int CurrentScreen { get; set; }
    }
}
