namespace NexFB.Models.Report.MealForecastReport
{
    using System;

    [Serializable]
    public class TotalMealForecastModel
    {
        public int Registration { get; set; }
        public int NonRegistration { get; set; }

        public int ActualOrder { get; set; }

        public double Efficiency
        {
            get
            {
                double efficiency;
                if (ActualOrder < 0)
                {
                    efficiency = -1;
                }
                else
                {
                    if (Registration.Equals(0))
                    {
                        efficiency = -1;
                    }
                    else
                    {
                        const int Hundred = 100;
                        efficiency = Math.Round((double)ActualOrder / Registration * Hundred);
                    }
                }

                return efficiency;
            }
        }
    }
}