using MauiApp1.Models.PatientModels;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public interface IAPIService
    {
        Task<Patient?> PostPatientAsync(string method); 
        // You can add more tasks for each model as needed
    }

    public class RequestManager : IAPIService
    {
        private readonly string baseURL = "http://ddnd.crabdance.com:80";
        private readonly HttpClient _httpClient;

        public RequestManager()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Patient?> PostPatientAsync(string method)
        {
            try
            {
                // Create an empty JSON object (if needed, populate with necessary data)
                JsonObject jObject = new JsonObject();

                var jsonResponse = await AbstractRequestAsync("/patient", jObject);
                var data = JsonSerializer.Deserialize<Patient>(jsonResponse);

                if (data == null)
                {
                    Console.WriteLine("Failed to deserialize the response.");
                    return null;
                }

                return data;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }

        /*public async Task<>*/

        public async Task<string> AbstractRequestAsync(string method, JsonObject parameters)
        {
            var stringC = new StringContent(parameters.ToString(), Encoding.UTF8, "application/json"); // Specify content-type
            HttpResponseMessage response = await _httpClient.PostAsync(baseURL + method, stringC);
            response.EnsureSuccessStatusCode(); // Throws exception if not 2xx success code

            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            

            return jsonResponse;
        }

    }
}
