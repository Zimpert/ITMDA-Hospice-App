using System.Diagnostics;
using System.Text;

namespace MauiApp1.Classes
{
    /// <summary>
    /// Class to handle API requests.
    /// </summary>
    public class ApiRequest
    {
        private HttpClient _client;
        protected readonly string baseURL = "http://ddnd.crabdance.com"; // Base URL for the API

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequest"/> class.
        /// </summary>
        public ApiRequest()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Sends an asynchronous POST request to the specified endpoint with the provided JSON content.
        /// </summary>
        /// <param name="endpoint">The API endpoint to send the request to.</param>
        /// <param name="jsonContent">The JSON content to include in the request body.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response content as a string.</returns>
        /// <exception cref="ArgumentException">Thrown when the JSON content is empty or null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while sending the request or an unexpected error occurs.</exception>
        public async Task<string> SendRequestAsync(string endpoint, string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Console.WriteLine("JSON content is empty or null.");
                throw new ArgumentException("JSON content cannot be empty or null.");
            }

            var fullUrl = baseURL + endpoint;
            Console.WriteLine($"Request URL: {fullUrl}");

            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Response Status Code: {response.StatusCode}");

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");

                return responseContent;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                throw new InvalidOperationException("An error occurred while sending the request.", e);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw new InvalidOperationException("An unexpected error occurred.", ex);
            }
        }

        /// <summary>
        /// Checks if the server is available by sending a HEAD request to the base URL. For Debugging Only
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the server is available.</returns>
        //private async Task<bool> IsServerAvailableAsync()
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Head, baseURL);
        //        HttpResponseMessage response = await _client.SendAsync(request);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }

}
