using System.Diagnostics;
using System.Text;

namespace MauiApp1.Classes
{
    public class ApiRequest
    {

        private HttpClient _client;
        protected readonly string baseURL = "http://ddnd.crabdance.com"; // Base URL for the API

        public ApiRequest()
        {
            _client = new HttpClient();
        }

        public async Task<string> SendRequestAsync(string endpoint, string jsonContent)
        {
            Debug.WriteLine("SendRequestStart");

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Debug.WriteLine("JSON content is empty or null.");
                throw new ArgumentException("JSON content cannot be empty or null.");
            }

            if (!await IsServerAvailableAsync())
            {
                Debug.WriteLine("Server is not available.");
                throw new InvalidOperationException("Server is not available.");
            }

            var fullUrl = baseURL + endpoint;
            Debug.WriteLine($"Request URL: {fullUrl}");
            Debug.WriteLine($"Request Content: {jsonContent}");

            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            // Additional debugging information
            Debug.WriteLine($"Request Method: {request.Method}");
            Debug.WriteLine($"Request Headers: {request.Headers}");
            Debug.WriteLine($"Request Content Headers: {request.Content.Headers}");
            Debug.WriteLine($"Request Content: {await request.Content.ReadAsStringAsync()}");

            Debug.WriteLine("SendRequestMiddle");

            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
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

        private async Task<bool> IsServerAvailableAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, baseURL);
                HttpResponseMessage response = await _client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

}
