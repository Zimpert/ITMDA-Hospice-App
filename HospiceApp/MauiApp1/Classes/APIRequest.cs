using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MauiApp1.Classes
{
    public class ApiRequest
    {
        protected static readonly HttpClient client = new HttpClient(new HttpClientHandler());
        protected readonly string baseURL = "http://ddnd.crabdance.com"; // Base URL for the API

        public async Task<string> SendRequestAsync(string endpoint, string jsonContent)
        {
            Debug.WriteLine("SendRequestStart");

            var fullUrl = baseURL + endpoint;
            Debug.WriteLine($"Request URL: {fullUrl}");
            Debug.WriteLine($"Request Content: {jsonContent}");

            if (string.IsNullOrEmpty(jsonContent))
            {
                Debug.WriteLine("Request content is empty.");
                return string.Empty;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            Debug.WriteLine("SendRequestMiddle");

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30))) // 30-second timeout for this request
            {
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request, cts.Token);
                    Debug.WriteLine("SendRequestEnd");

                    response.EnsureSuccessStatusCode();
                    Debug.WriteLine($"Response Status Code: {response.StatusCode}");

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {responseContent}");

                    return responseContent;
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("Request timed out.");
                    return string.Empty;
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine($"Request error: {e.Message}");
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unexpected error: {ex.Message}");
                    return string.Empty;
                }
            }
        }
    }
}
