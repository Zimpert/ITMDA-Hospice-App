using System.Diagnostics;
using System.Text;

namespace MauiApp1.Classes
{
    public class ApiRequest
    {

        //protected static readonly HttpClient client = new HttpClient();
        protected static readonly HttpClient client = new HttpClient(new HttpClientHandler());
        protected readonly string baseURL = "http://ddnd.crabdance.com"; // Base URL for the API

        public async Task<string> SendRequestAsync(string endpoint, string jsonContent)
        {
            Debug.WriteLine("SendRequestStart");

            var fullUrl = baseURL + endpoint;
            Debug.WriteLine($"Request URL: {fullUrl}");
            Debug.WriteLine($"Request Content: {jsonContent}");

            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            Debug.WriteLine("SendRequestMiddle");

            try
            {
            
                HttpResponseMessage response = await client.SendAsync(request);
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
