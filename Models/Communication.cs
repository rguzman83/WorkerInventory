using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInventory.Models.Communication
{
    public class CommunicationResponse
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
        public string? CommunicationId { get; set; }
        public string? Type { get; set; }
        public string? UsageType { get; set; }
        public string? DialCountry { get; set; }
        public string? DialArea { get; set; }
        public string? DialNumber { get; set; }
        public string? DialExtension { get; set; }
        public string? StreetLineOne { get; set; }
        public string? StreetLineTwo { get; set; }
        public string? PostOfficeBox { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? CountrySubdivisionCode { get; set; }
        public string? CountryCode { get; set; }
        public string? Uri { get; set; }
    }


}
