using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Classes
{
    public class ApiRequest
    {
        protected static readonly HttpClient client = new HttpClient();
        protected readonly string baseURL = "http://ddnd.crabdance.com"; // Base URL for the API

        public async Task<string> SendRequestAsync(string endpoint, string jsonContent)
        {
            var fullUrl = baseURL + endpoint;
            var request = new HttpRequestMessage(HttpMethod.Post, fullUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

}
