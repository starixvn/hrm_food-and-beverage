namespace NexFB.Models.Report.MealForecastReport
{
    using System;

    [Serializable]
    public class MealForecastItemModel
    {
        private const int Hundred = 100;

        public DateTime OnDate { get; set; }
        public int NonRegistrationVegetarian { get; set; }
        public int NonRegistrationNonVegetarian { get; set; }

        public int RegistrationNonVegetarian { get; set; }

        public int ActualOrderNonVegetarian { get; set; }

        public double EfficiencyNonVegetarian
        {
            get
            {
                double efficiency;
                if (ActualOrderNonVegetarian < 0)
                {
                    efficiency = -1;
                }
                else
                {
                    if (RegistrationNonVegetarian.Equals(0))
                    {
                        efficiency = -1;
                    }
                    else
                    {
                        efficiency = Math.Round((double)ActualOrderNonVegetarian / RegistrationNonVegetarian * Hundred);
                    }
                }

                return efficiency;
            }
        }

        public int RegistrationVegetarian { get; set; }

        public int ActualOrderVegetarian { get; set; }

        public double EfficiencyVegetarian
        {
            get
            {
                double efficiency;
                if (ActualOrderVegetarian < 0)
                {
                    efficiency = -1;
                }
                else
                {
                    if (RegistrationVegetarian.Equals(0))
                    {
                        efficiency = -1;
                    }
                    else
                    {
                        efficiency = Math.Round((double)ActualOrderVegetarian / RegistrationVegetarian * Hundred);
                    }
                }

                return efficiency;
            }
        }
    }
}