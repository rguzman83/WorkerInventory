using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInventory.Models.Organizations
{
    public class OrganizationsResponse
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
        public string? OrganizationId { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
        public string? Level { get; set; }
    }

    


}
