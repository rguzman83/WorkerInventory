using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerInventory.Models.Company;
using WorkerInventory.Models.LaborAssignment;
using WorkerInventory.Models.Organizations;
using WorkerInventory.Repositories.Company;
using WorkerInventory.Repositories.JSON;

namespace WorkerInventory.Repositories.Company
{
    public class CompanyRepository
    {
        public async Task<CompanyResponse> GetCompanys(string strToken, string strBaseURL) // Get all companies
        {
            CompanyResponse companyResponse = new CompanyResponse();
            if (strToken is null)
            {
                Console.WriteLine("[<< No auth token provided >>]");
            }
            else
            {
                strToken = "Bearer " + strToken;
                var baseAddress = new Uri(strBaseURL);

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {


                    httpClient.DefaultRequestHeaders.Add("Authorization", strToken);

                    try
                    {
                        // Send a POST request to the specified Uri as an asynchronous operation.
                        HttpResponseMessage response = await httpClient.GetAsync("/companies");

                        // Ensure we get a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response as a string.
                        string result = await response.Content.ReadAsStringAsync();

                        companyResponse = JSONRepository.ToClass<CompanyResponse>(result)!;

                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            return companyResponse;
        }

        public async Task<LaborAssignmentResponse> GetCompanyLaborAssignments(string strToken, string strBaseURL, string strCompanyId) // Get all company Labor Assignments
        {
            LaborAssignmentResponse laborAssignmentsResponse = new LaborAssignmentResponse();
            if (strToken is null)
            {
                Console.WriteLine("[<< No auth token provided >>]");
            }
            else
            {
                strToken = "Bearer " + strToken;
                var baseAddress = new Uri(strBaseURL);

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {


                    httpClient.DefaultRequestHeaders.Add("Authorization", strToken);

                    try
                    {
                        // Send a POST request to the specified Uri as an asynchronous operation.
                        HttpResponseMessage response = await httpClient.GetAsync($"/companies/{strCompanyId}/laborassignments");

                        // Ensure we get a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response as a string.
                        string result = await response.Content.ReadAsStringAsync();

                        laborAssignmentsResponse = JSONRepository.ToClass<LaborAssignmentResponse>(result)!;

                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            return laborAssignmentsResponse;
        }

        public async Task<OrganizationsResponse> GetCompanyOrgs(string strToken, string strBaseURL, string strCompanyId) // Get all company Org Structure
        {
            OrganizationsResponse organizationsResponse = new OrganizationsResponse();
            if (strToken is null)
            {
                Console.WriteLine("[<< No auth token provided >>]");
            }
            else
            {
                strToken = "Bearer " + strToken;
                var baseAddress = new Uri(strBaseURL);

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {


                    httpClient.DefaultRequestHeaders.Add("Authorization", strToken);

                    try
                    {
                        // Send a POST request to the specified Uri as an asynchronous operation.
                        HttpResponseMessage response = await httpClient.GetAsync($"/companies/{strCompanyId}/organizations");

                        // Ensure we get a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response as a string.
                        string result = await response.Content.ReadAsStringAsync();

                        organizationsResponse = JSONRepository.ToClass<OrganizationsResponse>(result)!;

                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            return organizationsResponse;
        }
    }
}
