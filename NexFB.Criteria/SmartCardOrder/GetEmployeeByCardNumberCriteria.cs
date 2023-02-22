namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;

    public class GetEmployeeByCardNumberCriteria : CriteriaBase
    {
        public string CardNumber { get; set; }

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CardNumber);

        public override string GetSettingKey() => "Employee_GetEmployeeByCardNumber";
    }
}