namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MealForecastDetailModel
    {
        public int MenuId { get; set; }

        public string MenuVie { get; set; }

        public string MenuEng { get; set; }

        public List<MealModel> Meals { get; set; } = new List<MealModel>();

        public bool IsVegetarian { get; set; }

        public int Registration { get; set; }
        public int NonRegistration { get; set; }

        public int ActualOrder { get; set; }

        public double OrderRate
        {
            get
            {
                double orderRate;
                if (ActualOrder < 0)
                {
                    orderRate = -1;
                }
                else
                {
                    if (Registration.Equals(0))
                    {
                        orderRate = -1;
                    }
                    else
                    {
                        const int Hundred = 100;
                        orderRate = Math.Round((double)ActualOrder / Registration * Hundred);
                    }
                }

                return orderRate;
            }
        }
    }
}