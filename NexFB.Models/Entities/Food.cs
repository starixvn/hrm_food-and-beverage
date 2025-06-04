namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// Food Item.
    /// </summary>
    [Serializable]
    public class Food
    {
        public int FoodId { get; set; }

        public string Name 
        {
            get
            {
                return NameVN;
            }
        }

        public string NameVN { get; set; }

        public string NameEN { get; set; }
    }
}
