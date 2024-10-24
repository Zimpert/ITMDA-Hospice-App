using MauiApp1.Models.PatientModels;
using MauiApp1.ViewModels;
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
        Task<CaregiverShiftViewModel?> GetCaregiverShifts (string method);
    }

    public class RequestManager : IAPIService
    {
        private readonly string baseURL = "http://ddnd.crabdance.com:80";
        private readonly HttpClient _httpClient;

        public RequestManager()
        {
            _httpClient = new HttpClient();
        }

        public async Task<CaregiverShiftViewModel?> GetCaregiverShifts(string method)
        {
            try
            {
                // Create an empty JSON object (if needed, populate with necessary data)
                var jObject = new
                { 
                    CaregiverID = "1"
                };

                string json = JsonSerializer.Serialize(jObject);


                var jsonResponse = await AbstractRequestAsync("/caregivershift", json);
                var data = JsonSerializer.Deserialize<CaregiverShiftViewModel>(jsonResponse);

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

        public async Task<Patient?> PostPatientAsync(string method)
        {
            try
            {
                // Create an empty JSON object (if needed, populate with necessary data)
                var jObject = "";

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

        public async Task<string> AbstractRequestAsync(string method, string parameters)
        {
            var stringC = new StringContent(parameters, Encoding.UTF8, "application/json"); // Specify content-type
            HttpResponseMessage response = await _httpClient.PostAsync(baseURL + method, stringC);
            response.EnsureSuccessStatusCode(); // Throws exception if not 2xx success code

            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            

            return jsonResponse;
        }

    }
}
