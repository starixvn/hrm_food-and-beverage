namespace NexFB.Models.ShiftMealMapping
{
    public class ShiftMealMappingModel
    {
        public int ShiftId { get; set; }

        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }

        public string BeginCheckInTime { get; set; }

        public string EndCheckInTime { get; set; }

        public string BeginCheckOutTime { get; set; }

        public string EndCheckOutTime { get; set; }

        public bool MealPreviousSupper { get; set; }

        public bool MealBreakfast { get; set; }

        public bool MealLunch { get; set; }

        public bool MealDinner { get; set; }

        public bool MealSupper { get; set; }

        public bool MealNextBreakfast { get; set; }

        public bool MealNextLunch { get; set; }

        public bool MealNextDinner { get; set; }

        public short CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}