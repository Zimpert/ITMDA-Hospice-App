using MauiApp1.Classes;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using MauiApp1.ViewModels;
using System.Text.Json;

namespace MauiApp1.Services
{
    /// <summary>
    /// Manages requests to the server.
    /// </summary>
    public class RequestManager : IRequestManager
    {
        private readonly AbstractRequest _abstractRequest;
        public RequestManager(AbstractRequest abstractRequest)
        {
            _abstractRequest = abstractRequest;
        }


        public async Task<User?> Login(string method, string email, string password)
        {
            try
            {
                var jObject = new
                {
                    Email = email,
                    Password = password
                };
                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _abstractRequest.AbstractRequestAsync("/login", json);
                var data = JsonSerializer.Deserialize<User>(jsonResponse);

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

        public Task<Patient?> PostPatientAsync(string method)
        {
            throw new NotImplementedException();
        }
    }


}
