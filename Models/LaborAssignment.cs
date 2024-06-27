using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInventory.Models.LaborAssignment
{
    public class LaborAssignmentResponse
    {
        public Metadata? Metadata { get; set; }
        public IList<Content>? Content { get; set; }
    }


    public class Metadata
    {
        public int? ContentItemCount { get; set; }
    }

    public class Content
    {
        public string? LaborAssignmentId { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? OrganizationId { get; set; }
        public string? LocationId { get; set; }
        public string? PositionId { get; set; }
    }

    


}
