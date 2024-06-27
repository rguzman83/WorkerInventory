using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerInventory.Models.Employee;
using WorkerInventory.Models.Communication;
using WorkerInventory.Models.Compensation;

namespace WorkerInventory.Models.CombinedWorker
{
    public class CombinedWorker // container for all 3 core sections - ALL data. EVERYTHING.
    {
        public int? WorkerCount { get; set; }
        public IList<WorkerRecord>? Worker { get; set; }
    }

    public class WorkerRecord
    {
        Employee.Content? CombEmployee {  get; set; }
        IList<Communication.Content>? CombCommunications {  get; set; }
        IList<Compensation.PayRateContent>? CombCompensation { get; set; }
    }
}
