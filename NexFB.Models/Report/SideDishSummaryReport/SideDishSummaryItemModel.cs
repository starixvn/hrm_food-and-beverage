namespace NexFB.Models.Report.SideDishSummaryReport
{
    using System;

    /// <summary>
    /// Side dish summary item model.
    /// </summary>
    [Serializable]
    public class SideDishSummaryItemModel
    {
        public int SysId { get; set; }

        public string CompanyTeamName { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string EmpType { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }
    }
}
