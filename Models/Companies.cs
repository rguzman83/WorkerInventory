using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.ComponentModel;

namespace WorkerInventory.Models.Company
{
    public class CompanyResponse
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
        public string? CompanyId { get; set; }
        public string? DisplayId { get; set; }
        public string? LegalName { get; set; }
        public bool? HasPermission { get; set; }
        public LegalId? LegalId { get; set; }
        public IList<Communication>? Communications { get; set; }
    }

    public class LegalId
    {
        public string? LegalIdType { get; set; }
        public string? LegalIdValue { get; set; }
    }

    public class Communication
    {
        public string? Type { get; set; }
        public string? UsageType { get; set; }
        public string? StreetLineOne { get; set; }
        public string? StreetLineTwo { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? CountrySubdivisionCode { get; set; }
        public string? CountryCode { get; set; }
    }

}
