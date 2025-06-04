namespace NexFB.Models.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Menu Entity.
    /// </summary>
    [Serializable]
    public class Menu
    {
        public int MenuId { get; set; }

        public string MenuCode { get; set; }

        public string NameVN { get; set; }

        public string NameEN { get; set; }

        public bool IsMainMenu { get; set; }

        public bool IsHaveAlternativeMenu { get; set; }

        public bool IsSideDish { get; set; }

        public int QuantityOrder { get; set; }

        public decimal Price { get; set; }

        public decimal TotalAmount
        {
            get
            {
                return QuantityOrder * Price;
            }
        }

        public string Name
        {
            get
            {
                return NameEN;
            }
        }

        public IList<Food> FoodItems { get; set; }

        public string FoodSetVNMName { get; set; }
    }
}
