namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeNonRegistrationCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "WorkDate")]
        public DateTime Date { get; set; }

        public int? MealId { get; set; }

        public int? MenuId { get; set; }

        public string CompanyId { get; set; }

        public string EmployeeTypeId { get; set; }

        public bool? IsVegetarian { get; set; }

        [SpParam(Ignored = true)]
        public int StartIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; } = 50;

        [SpParam(Ignored = true)]
        public bool IsPagingCallback { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_NonRegistrationMealDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}