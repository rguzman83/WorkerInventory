#nullable enable
#pragma warning disable CS8618
#pragma warning disable CS8602

using WorkerInventory.Models.Company;
using WorkerInventory.Models.Organizations;
using WorkerInventory.Models.LaborAssignment;
using WorkerInventory.Models.Employee;
using WorkerInventory.Models.Compensation;
using WorkerInventory.Models.CombinedWorker;
using WorkerInventory.Models.WorkerObject;
using WorkerInventory.Repositories.Authorization;
using WorkerInventory.Repositories.Company;
using WorkerInventory.Repositories.Employee;
using WorkerInventory.Repositories.JSON;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using System.Reflection.Metadata;
using System.ComponentModel;
using System.Diagnostics;

namespace WorkerInventory
{
    internal class Program
    {
        string strMainMenuChoice;
        string strCompanySelectionChoice;
        string strToken;
        string strBaseURL;
        string strCompanyId;
        int intCompanySelection;


        static void Main(string[] args)
        {
            Program myProg = new Program();
            myProg.Run();
        }

        public void Run()
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            CompanyRepository companyRepository = new CompanyRepository();
            AuthorizationRepository authorizationRepository = new AuthorizationRepository();

            CompanyResponse companies = new CompanyResponse(); // Store all inbound company IDs - Currently unused
            LaborAssignmentResponse laborAssignments = new LaborAssignmentResponse(); // Store all Labor Assignment Details
            OrganizationsResponse organizations = new OrganizationsResponse(); // Store all Org Level Details
            EmployeeResponse workerDemographics = new EmployeeResponse(); // Stores all the workers Demographics (and optionally Comms)
            CompensationResponse workerPayRates = new CompensationResponse(); // Stores all the workers Compensation (Pay Rates) data
            

            //var combinedWorkerRecord = new List<CombinedWorker>();
            List<WorkerInventoryObject> workerInventory = new List<WorkerInventoryObject>(); //Stores worker inventory formatted data as a List

            

            bool blMainMenuComplete = false;
            while (!blMainMenuComplete)
            {  //┌─┐┘└│
                if(strToken == null)
                    Console.WriteLine($"[<< Expected Credentials File Path - {Directory.GetCurrentDirectory()}\\credentials.txt >>] ");
                Console.WriteLine("┌─────────────────────────────────────┐");
                Console.WriteLine("│           ---[OPTIONS]---           │");
                Console.WriteLine("│ 1 - Authenticate                    │");
                Console.WriteLine("│ 2 - Get All Companies               │");
                Console.WriteLine("│ 3 - Get Workers With Communications │");
                Console.WriteLine("│ 4 - Get Workers Compensation Data   │");
                Console.WriteLine("│ 5 - List Workers (Not Needed)       │");
                Console.WriteLine("│ 6 - Create Worker Inventory         │");
                Console.WriteLine("│ 7 - Write Inventory to File         │");
                Console.WriteLine("│ Q - Quit                            │");
                Console.WriteLine("└─────────────────────────────────────┘");
                strMainMenuChoice = Console.ReadLine()!;

                if (strMainMenuChoice != null)
                {
                    switch (strMainMenuChoice.ToUpper())
                    {
                        case "1":
                            var authAttempt = authorizationRepository.APIAuthenticate().Result;
                            strToken = authAttempt.Item1;
                            strBaseURL= authAttempt.Item2;
                            break;
                        case "2":
                            if (strToken != null)
                            {

                                companies = companyRepository.GetCompanys(strToken, strBaseURL).Result;
                                if (companies.Content != null)
                                {
                                    bool blCompanySelectionComplete = false;
                                    while (!blCompanySelectionComplete)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("[<< Companies Obtained >>]");
                                        Console.WriteLine("[<< Listing Results >>]");
                                        Console.WriteLine("┌──────────────────────────────────────┐");
                                        Console.WriteLine("│  [<< Pick Which Company to Load >>]  │");
                                        Console.WriteLine("└──────────────────────────────────────┘");
                                        for (int i = 0; i != companies.Content.Count; i++)
                                        {
                                            Console.WriteLine($"Company [{i}] - {companies.Content[i].LegalName}");
                                        }

                                        if (int.TryParse(Console.ReadLine(), out intCompanySelection)) // only pass along if an int was entered
                                        {
                                            if (intCompanySelection >= 0 && intCompanySelection <= (companies.Content.Count - 1)) // only trigger if our value is a valid
                                                blCompanySelectionComplete = true;
                                        }

                                        

                                    }
                                    strCompanyId = companies.Content[intCompanySelection].CompanyId!;
                                    Console.Clear();
                                    Console.WriteLine($"[<<< Company {companies.Content[intCompanySelection].LegalName} | {strCompanyId} is Loading >>]");
                                    
                                    laborAssignments = companyRepository.GetCompanyLaborAssignments(strToken, strBaseURL, strCompanyId).Result;
                                    if(laborAssignments.Content != null )
                                        Console.WriteLine($"[<< {laborAssignments.Metadata.ContentItemCount} Labor Assignments Loaded >>]");
                                    organizations = companyRepository.GetCompanyOrgs(strToken, strBaseURL, strCompanyId).Result;
                                    if( organizations.Content != null )
                                        Console.WriteLine($"[<< {organizations.Metadata.ContentItemCount} Organizations Loaded >>]");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("[<< Failed to obtain Companies >>]");
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("[<< No Token Present >>]");
                            }
                            break;
                        case "3":
                            if (strToken != null && strCompanyId != null)
                            {
                                Console.Clear();
                                Console.WriteLine("[<< Attempting Worker Retrieval >>]");
                                workerDemographics = employeeRepository.GetWorkersWithCommunications(strToken, strBaseURL, strCompanyId).Result;

                                if (workerDemographics.Content != null)
                                {
                                    Console.WriteLine($"[<< Worker Count = {workerDemographics.Content.Count} >>]");
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("[<< One or more required items is null >>]");
                            }
                            break;
                        case "4":
                            if (strToken != null && strCompanyId != null)
                            {
                                ConsoleKey getCompensationResponse;
                                do // Loop the following menu until the user gives a Y or N response
                                {
                                    Console.Clear();
                                    //┌─┐┘└│
                                    Console.WriteLine("┌─────────────────────────────┐");
                                    Console.WriteLine("│        ---CAUTION---        │");
                                    Console.WriteLine("│  This is OPTIONAL and will  │");
                                    Console.WriteLine("│   take a WHILE to process   │");
                                    Console.WriteLine("│                             │");
                                    Console.WriteLine("│        Are you SURE?        │");
                                    Console.WriteLine("│        [Y]es or [N]o        │");
                                    Console.WriteLine("└─────────────────────────────┘");

                                    getCompensationResponse = Console.ReadKey(false).Key;
                                } while (getCompensationResponse != ConsoleKey.Y &&  getCompensationResponse != ConsoleKey.N);

                                if(getCompensationResponse == ConsoleKey.Y)
                                {
                                    Console.WriteLine("[<< Attempting Worker Compensation >>]");
                                    workerPayRates = employeeRepository.GetWorkerPayRates(strToken, strBaseURL, workerDemographics).Result;

                                    if (workerPayRates.Content != null)
                                    {
                                        Console.WriteLine($"[<< Compensation Count = {workerPayRates.Content.Count} >>]");
                                    }
                                }
                                if (getCompensationResponse == ConsoleKey.N)
                                    Console.Clear();
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("[<< One or more required items is null >>]");
                            }
                            break;
                        case "5":
                            if(workerDemographics.Content != null)
                            {
                                bool listComplete = false;
                                int i = 0;
                                int intMaxWorkers = workerDemographics.Content.Count;

                                while (!listComplete)
                                {
                                    Console.Clear();
                                    Console.WriteLine("[<< Listing Workers >>]");
                                    for (int j = i; j != i + 3; j++)
                                    {
                                        if (j < intMaxWorkers)
                                        {
                                            if (workerDemographics.Content[j].Communications != null)
                                            {
                                                Console.WriteLine($"Name {j + 1} = {workerDemographics.Content[j].Name!.GivenName} {workerDemographics.Content[j].Name!.FamilyName} | " +
                                                    $"Has {workerDemographics.Content[j].Communications!.Count} Communications");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Name {j + 1} = {workerDemographics.Content[j].Name!.GivenName} {workerDemographics.Content[j].Name!.FamilyName} | " +
                                                    $"Has 0 Communications");
                                            }
                                        }
                                    }
                                    Console.WriteLine("[P]revious 3");
                                    Console.WriteLine("[N]ext 3");
                                    Console.WriteLine("[S]top");
                                    strCompanySelectionChoice = Console.ReadLine()!;
                                    if (strCompanySelectionChoice != null)
                                    {
                                        switch (strCompanySelectionChoice.ToUpper())
                                        {
                                            case "P":
                                                if (i - 3 <= 0)
                                                    i = 0;
                                                else
                                                    i = i - 3;
                                                break;
                                            case "N":
                                                if (i + 3 >= intMaxWorkers)
                                                    i = i + (intMaxWorkers - i);
                                                else
                                                    i = i + 3;
                                                break;
                                            case "S":
                                                Console.Clear ();
                                                listComplete = true;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("[<< There are no workers present >>]");
                            }
                            break;
                        case "6":
                            if (workerDemographics.Content != null)
                            {
                                workerInventory.Clear(); // RESET INVENTORY

                                foreach (var partialEmployee in workerDemographics.Content)
                                {
                                    //Storeing blank values for everything because nulls are hated apperantly
                                    string employeeId = "NONE";
                                    string wType = "NONE";
                                    string eType = "NONE";
                                    string BEUri = "NONE";
                                    string PEUri = "NONE";
                                    string LEUri = "NONE";
                                    string orgId = "NONE";
                                    string laborId = "NONE";
                                    string orgName = "NONE";
                                    string laborName = "NONE";
                                    string rate1type = "NONE";
                                    double rate1amount = 0.00;

                                    // Get Buisiness & Personal Email Communication records (or NULL if not present)
                                    var BEComm = partialEmployee.Communications.SingleOrDefault(c => c.Type == "EMAIL" && c.UsageType == "BUSINESS");
                                    var PEComm = partialEmployee.Communications.SingleOrDefault(c => c.Type == "EMAIL" && c.UsageType == "PERSONAL");
                                    // If Org info exists, get it
                                    if (partialEmployee.Organization != null)
                                    {
                                        orgId = partialEmployee.Organization.OrganizationId;
                                        var org = organizations.Content.SingleOrDefault(o => o.OrganizationId == partialEmployee.Organization.OrganizationId);
                                        if (org != null)
                                            orgName = org.Name!;
                                    }
                                    //If Labor info exists, get it
                                    if (partialEmployee.LaborAssignmentId != null)
                                    {
                                        laborId = partialEmployee.LaborAssignmentId;
                                        var labor = laborAssignments.Content.SingleOrDefault(l => l.LaborAssignmentId == partialEmployee.LaborAssignmentId);
                                        if (labor != null)
                                            laborName = labor.Name!;

                                    }
                                    // If worker has pay rates, get RATE_1
                                    if (workerPayRates.Content != null)
                                    {
                                        var rate1 = workerPayRates.Content.SingleOrDefault(r => r.WorkerId == partialEmployee.WorkerId && r.RateNumber == "RATE_1");
                                        if (rate1 != null)
                                        {
                                            rate1type = rate1.RateType;
                                            rate1amount = Convert.ToDouble(rate1.Amount);
                                        }
                                    }
                                    // Processing in REVERSE desired order. We want "Personal" then "Business" else "NONE"
                                    // We Already have "NONE" saved for "Linkable" (LEUri) - we will check for Business first and update if it exists
                                    if (BEComm != null)
                                    {
                                        BEUri = BEComm.Uri!;
                                        LEUri = BEComm.Uri!; // Update from "NONE" to "Business"
                                    }
                                    // We now already have "NONE" OR "Buisness" email saved for "Linkable" (LEUri) - now we see if we have Personal and update if yes
                                    if (PEComm != null)
                                    {
                                        PEUri = PEComm.Uri!;
                                        LEUri = PEComm.Uri!; // Update from "NONE" or "Business" to "Personal"
                                    }

                                    if (partialEmployee.EmployeeId != null)
                                        employeeId = partialEmployee.EmployeeId!;
                                    if (partialEmployee.WorkerType != null)
                                        wType = partialEmployee.WorkerType!;
                                    if (partialEmployee.EmploymentType != null)
                                        eType = partialEmployee.EmploymentType!;


                                    workerInventory.Add(new WorkerInventoryObject
                                    {
                                        WorkerId = partialEmployee.WorkerId,
                                        FirstName = partialEmployee.Name.GivenName,
                                        LastName = partialEmployee.Name.FamilyName,
                                        EmployeeId = employeeId,
                                        WorkerType = wType,
                                        EmploymentType = eType,
                                        Status = partialEmployee.CurrentStatus.StatusType,
                                        OrgId = orgId,
                                        OrgName = orgName,
                                        LaborAssignmentID = laborId,
                                        LaborAssignmentName = laborName,
                                        Rate1Type = rate1type,
                                        Rate1Amount = rate1amount,
                                        BusinessEmail = BEUri,
                                        PersonalEMail = PEUri,
                                        LinkableEmail = LEUri

                                    });


                                }
                                Console.Clear();
                                Console.WriteLine($"[<< Number of workers in Inventory - {workerInventory.Count} >>]");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("[<< There are no workers present >>]");
                            }
                            break;
                        case "7":
                            if (workerInventory != null && workerInventory.Count > 0) //Only write file if not null and has workers
                            {
                                string jsonData = JsonSerializer.Serialize(workerInventory, new JsonSerializerOptions { WriteIndented = true });
                                // File.WriteAllText "Creates a new file, writes the specified string to the file, and then closes the file.
                                //                    If the target file already exists, it is overwritten."
                                File.WriteAllText(@"WorkerInv.json", jsonData); //Writes file to current working/default directory
                                Console.Clear();
                                Console.WriteLine($"[<< File written to {Directory.GetCurrentDirectory()}\\WorkerInv.json >>]"); //User feedback on where the file is
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("[<< There is no Worker Inventory Data >>]");
                            }
                            break;
                        case "Q":
                            blMainMenuComplete = true;
                            break;
                        default:
                            Console.Clear();
                            break;
                    }

                }
            }
        }

    }
    
    

    
}
