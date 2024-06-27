using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerInventory.Models.Employee;
using WorkerInventory.Models.Compensation;
using WorkerInventory.Repositories.JSON;
using System.ComponentModel.Design;

namespace WorkerInventory.Repositories.Employee
{
    public class EmployeeRepository
    {
        public EmployeeResponse WorkerDemographics = new EmployeeResponse();
        
        public async Task<EmployeeResponse> GetWorkersDemographics(string strToken, string strBaseURL, string companyId)
        {
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
                        HttpResponseMessage response = await httpClient.GetAsync($"/companies/{companyId}/workers");

                        // Ensure we get a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response as a string.
                        string result = await response.Content.ReadAsStringAsync();

                        WorkerDemographics = JSONRepository.ToClass<EmployeeResponse>(result)!;

                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            return WorkerDemographics;
        }

        public async Task<EmployeeResponse> GetWorkersWithCommunications(string strToken, string strBaseURL, string companyId)
        {
            if (strToken is null)
            {
                Console.WriteLine("[<< No auth token provided >>]");
            }
            else
            {
                strToken = "Bearer " + strToken;
                var baseAddress = new Uri(strBaseURL);
                string strAcceptHeader = "application/json;profile='http://api.paychex.com/profiles/workers_communications/v1'";

                Console.WriteLine($"[<< Header = {strAcceptHeader} >>]");

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {

                   
                    httpClient.DefaultRequestHeaders.Add("Authorization", strToken);
                    //httpClient.DefaultRequestHeaders.Add("Accept", strAcceptHeader); // Fails with invalid format because of the "=" - or rather, because "profile" is not a defined scheme
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", strAcceptHeader); // bypasses scheme validation - could also create & define a new scheme

                    /*
                    Example of defining a new scheme
                    var authenticationHeaderValue = new AuthenticationHeaderValue("some scheme", "some value");
                    client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
                     */


                    try
                    {
                        // Send a POST request to the specified Uri as an asynchronous operation.
                        HttpResponseMessage response = await httpClient.GetAsync($"/companies/{companyId}/workers");

                        // Ensure we get a successful response.
                        response.EnsureSuccessStatusCode();

                        // Read the response as a string.
                        string result = await response.Content.ReadAsStringAsync();

                        WorkerDemographics = JSONRepository.ToClass<EmployeeResponse>(result)!;

                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                }
            }
            return WorkerDemographics;
        }

        public async Task<CompensationResponse> GetWorkerPayRates (string strToken, string strBaseURL, EmployeeResponse workers)
        {
            List<PayRateContent> workerPayRatesResponse = new List<PayRateContent>();

            if (strToken is null)
            {
                Console.WriteLine("[<< No auth token provided >>]");
            }
            else
            {
                if (workers.Content == null)
                {
                    Console.WriteLine("[<< There are no workers to load >>]");
                }
                else
                {
                    int intTotalWorkers = workers.Content.Count;
                    int intCurrentWorker = 0;
                    strToken = "Bearer " + strToken;
                    var baseAddress = new Uri(strBaseURL);

                    using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                    {


                        httpClient.DefaultRequestHeaders.Add("Authorization", strToken);

                        foreach (var worker in workers.Content)
                        {
                            intCurrentWorker++;
                            Console.Clear();
                            Console.WriteLine($"[<< Loading worker {intCurrentWorker} of {intTotalWorkers} >>]");
                            string strWorkerId = worker.WorkerId;

                            try
                            {
                                // Send a POST request to the specified Uri as an asynchronous operation.
                                HttpResponseMessage response = await httpClient.GetAsync($"/workers/{strWorkerId}/compensation/payrates");

                                // Ensure we get a successful response.
                                response.EnsureSuccessStatusCode();

                                // Read the response as a string.
                                string result = await response.Content.ReadAsStringAsync();

                                CompensationResponse payRateResponse = JSONRepository.ToClass<CompensationResponse>(result)!;

                                if (payRateResponse.Content.Count > 0)
                                {
                                    for (int i = 0; i < payRateResponse.Content.Count; i++)
                                    {
                                        payRateResponse.Content[i].WorkerId = strWorkerId;
                                        workerPayRatesResponse.Add(payRateResponse.Content[i]);
                                    }
                                }

                        }
                            catch (HttpRequestException e)
                            {
                                Console.WriteLine("Error: " + e.Message);
                            }
                        }
                    }
                }
            }
                

            CompensationResponse fullWorkerPayrates = new CompensationResponse{Content = workerPayRatesResponse};
            //fullWorkerPayrates.Metadata.ContentItemCount = workerPayRates.Count;  // <- Optional Number, meaningless now? Remove?
            return fullWorkerPayrates;
        }

        public string GetWorkerCommunications (string strToken, string strBaseURL, string workerId)
        {
            return "Communications";
        }

        
    }
}
