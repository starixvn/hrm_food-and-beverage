namespace NexFB.Models.Ordering
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// OrderMember Model.
    /// </summary>
    [Serializable]
    public class OrderMemberModel
    {
        public OrderMemberModel()
        {
            Menus = new List<Menu>();
        }

        public int OrderId { get; set; }

        public Employee Member { get; set; }

        public IList<Menu> Menus { get; set; }

        public bool IsOrder { get; set; }

        public bool IsAllowOrder { get; set; }

        public Status CurrentStatus { get; set; }

        public bool? IsVegetarianRegister { get; set; }
        public bool IsAllowOrderByShift { get; set; }
    }
}
