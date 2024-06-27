using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkerInventory.Repositories.Authorization
{
    public class AuthorizationRepository
    {
        public async Task<(string, string)> APIAuthenticate()
        {
            string strToken = string.Empty;
            string strAPIKey = string.Empty;
            string strAPISecret = string.Empty;
            string strBaseURL = string.Empty;
            string strCredentialsFile = @$"{Directory.GetCurrentDirectory()}\credentials.txt";

            if (File.Exists(strCredentialsFile))
            {
                Console.Clear();
                Console.WriteLine("[<< Credentials File Exists >>]");
                string[] lines = File.ReadAllLines(strCredentialsFile);
                foreach (string line in lines)
                {
                    string[] items = line.Split("=");
                    if (items[0] != null && items[0] == "BaseURL" && items[1] != null)
                        strBaseURL = items[1];
                    if (items[0] != null && items[0] == "APIKey" && items[1] != null)
                        strAPIKey = items[1];
                    if (items[0] != null && items[0] == "APISecret" && items[1] != null)
                        strAPISecret = items[1];
                }

                Console.WriteLine($"APIKey = {strAPIKey}");
                Console.WriteLine($"APISecret = {strAPISecret}");

                var baseAddress = new Uri(strBaseURL);

                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    // Form data is typically sent as key-value pairs
                    var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", strAPIKey),
                new KeyValuePair<string, string>("client_secret", strAPISecret)
            };

                    // Encodes the key-value pairs for the ContentType 'application/x-www-form-urlencoded'
                    HttpContent content = new FormUrlEncodedContent(formData);

                    try
                    {
                        // Send a POST request to the specified Uri as an asynchronous operation.
                        //Console.WriteLine("Trying to Auth");
                        HttpResponseMessage response = await httpClient.PostAsync("/auth/oauth/v2/token", content);

                        // Ensure we get a successful response.
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine("[<< Auth Successful >>]");

                        // Read the response as a string.
                        string result = await response.Content.ReadAsStringAsync();
                        var doc = JsonDocument.Parse(result)!;
                        strToken = doc.RootElement.GetProperty("access_token").GetString()!;
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        strToken=string.Empty;
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("[<< Credentials File is Missing >>]");
            }

            

            return (strToken, strBaseURL);
        }
    }
    
}
