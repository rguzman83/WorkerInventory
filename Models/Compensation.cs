using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInventory.Models.Compensation
{
    public class CompensationResponse // This is acutally only the Pay Rates endpoint
    {
        public Metadata? Metadata { get; set; }
        public IList<PayRateContent>? Content { get; set; }
    }

    public class PayRateResponse
    {
        public IList<PayRateContent>? Content { get; set; }
    }
    public class Metadata
    {
        public int? ContentItemCount { get; set; }
    }

    public class PayRateContent
    {
        public string? WorkerId { get; set; }
        public string? RateId { get; set; }
        public DateTime? StartDate { get; set; }
        public string? RateNumber { get; set; }
        public string? RateType { get; set; }
        public string? Description { get; set; }
        public string? Amount { get; set; }
        public string? StandardHours { get; set; }
        public string? StandardOvertime { get; set; }
        public bool? Default { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }
}
