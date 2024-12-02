using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace MauiApp1.Classes
{
    public class ApiRequest
    {
        private readonly IHttpClientFactory _clientFactory;

        public ApiRequest(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> SendRequestAsync(string endpoint, string jsonContent)
        {
            Debug.WriteLine("SendRequestStart");

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Debug.WriteLine("JSON content is empty or null.");
                throw new ArgumentException("JSON content cannot be empty or null.");
            }

            // Get a new HttpClient instance
            var client = _clientFactory.CreateClient("API");

            Debug.WriteLine($"Request URL: {endpoint}");
            Debug.WriteLine($"Request Content: {jsonContent}");

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
                
            };

            // Additional debugging information
            Debug.WriteLine($"Request Method: {request.Method}");
            Debug.WriteLine($"Request Headers: {request.Headers}");
            Debug.WriteLine($"Request Content Headers: {request.Content.Headers}");
            Debug.WriteLine($"Request Content: {await request.Content.ReadAsStringAsync()}");
            Debug.WriteLine("SendRequestMiddle");

            try
            {
                if (endpoint == "/addtask" || endpoint == "/tasklog" || endpoint == "/medlog" || endpoint == "/shiftlog")
                {
                    try
                    {
                      await client.SendAsync(request);
                        Debug.WriteLine("It was sent");
                        return "Sent";
                    } catch (HttpRequestException e)
                    {
                        Debug.WriteLine("It was added to the db");
                        return "Sent";
                        
                    }

                }

                using var response = await client.SendAsync(request);
                Debug.WriteLine("SendRequestEnd");

                response.EnsureSuccessStatusCode();
                Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                string responseContent = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"Response Content: {responseContent}");

                return responseContent;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Request error: {e.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
    }

}
