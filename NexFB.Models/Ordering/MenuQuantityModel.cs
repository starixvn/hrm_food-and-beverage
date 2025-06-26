namespace NexFB.Models.Ordering
{
    using System;
    using NexFB.Models.Entities;

    /// <summary>
    /// MenuQuantity Model.
    /// </summary>
    [Serializable]
    public class MenuQuantityModel
    {
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public Menu Menu { get; set; }
    }
}
