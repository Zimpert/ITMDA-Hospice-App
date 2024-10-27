using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Classes
{
    public class AbstractRequest
    {
        private readonly HttpClient _httpClient; // HttpClient instance for making HTTP requests
        private readonly string baseURL = "http://ddnd.crabdance.com:80"; // Base URL for the API

        public AbstractRequest(HttpClient httpClient)
        {
            _httpClient = httpClient; // Initialize the HttpClient instance
        }

        public async Task<string> AbstractRequestAsync(string endpoint, string parameters)
        {
            // Create a StringContent object with the parameters, specifying UTF-8 encoding and JSON content type
            var stringC = new StringContent(parameters, Encoding.UTF8, "application/json");

            // Send a POST request to the specified endpoint with the provided parameters
            HttpResponseMessage response = await _httpClient.PostAsync(baseURL + endpoint, stringC);

            // Ensure the response indicates success (status code 2xx), otherwise throw an exception
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Return the response content
            return jsonResponse;
        }
    }

}
