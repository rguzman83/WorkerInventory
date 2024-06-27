using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInventory.Models.WorkerObject
{
    public class WorkerInventoryObject
    {
        public string? WorkerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeId { get; set; }
        public string? WorkerType { get; set; }
        public string? EmploymentType { get; set; }
        public string? Status { get; set; }
        public string? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? LaborAssignmentID { get; set; }
        public string? LaborAssignmentName { get; set; }
        public string? PersonalEMail { get; set; }
        public string? BusinessEmail { get; set; }
        public string? LinkableEmail { get; set; }
        public string? Rate1Type { get; set; }
        public double? Rate1Amount { get; set; }
    }
}



