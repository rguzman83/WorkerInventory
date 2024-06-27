using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerInventory.Models.Communication;

namespace WorkerInventory.Models.Employee
{
    public class EmployeeResponse
    {
        public Metadata? Metadata { get; set; }
        public IList<Content>? Content { get; set; }
    }


    public class Metadata
    {
        public int? ContentItemCount { get; set; }
    }

    public class Name
    {
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? MiddleName { get; set; }
    }

    public class LegalId
    {
        public string? LegalIdType { get; set; }
        public string? LegalIdValue { get; set; }
    }

    public class Job
    {
        public string? JobTitleId { get; set; }
        public string? Title { get; set; }
    }

    public class Organization
    {
        public string? OrganizationId { get; set; }
        public string? Name { get; set; }
        public string? Number { get; set; }
    }

    public class CurrentStatus
    {
        public string? WorkerStatusId { get; set; }
        public string? StatusType { get; set; }
        public string? StatusReason { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class Supervisor
    {
        public string? WorkerId { get; set; }
        public Name? Name { get; set; }
    }

    public class Content
    {
        public string? WorkerId { get; set; }
        public string? EmployeeId { get; set; }
        public string? WorkerType { get; set; }
        public string? ExemptionType { get; set; }
        public DateTime? HireDate { get; set; }
        public Name? Name { get; set; }
        public LegalId? LegalId { get; set; }
        public string? LaborAssignmentId { get; set; }
        public string? LocationId { get; set; }
        public string? JobId { get; set; }
        public Job? Job { get; set; }
        public Organization? Organization { get; set; }
        public CurrentStatus? CurrentStatus { get; set; }
        public string? EmploymentType { get; set; }
        public string? WorkState { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Sex { get; set; }
        public string? EthnicityCode { get; set; }
        public Supervisor? Supervisor { get; set; }
        public IList<Communication.Content>? Communications { get; set; } // Trying to add optional communication profile to worker details
    }
}
