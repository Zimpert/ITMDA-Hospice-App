using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Classes
{
    public class AbstractRequest
    {
        private readonly HttpClient _httpClient;
        private readonly string baseURL = "http://ddnd.crabdance.com:80"; // Base URL

        public AbstractRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AbstractRequestAsync(string method, string parameters)
        {
            var stringC = new StringContent(parameters, Encoding.UTF8, "application/json"); // Specify content-type
            HttpResponseMessage response = await _httpClient.PostAsync(baseURL + method, stringC);
            response.EnsureSuccessStatusCode(); // Throws exception if not 2xx success code
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }

}
