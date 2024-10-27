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

        public async Task<Caregiver?> GetCaregiverByIdAsync(string userId)
        {
            try
            {
                var jsonResponse = await _abstractRequest.AbstractRequestAsync($"/caregivers/{userId}", string.Empty);
                return JsonSerializer.Deserialize<Caregiver>(jsonResponse);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return null;
        }

        public async Task<Patient?> GetPatientByIdAsync(string userId)
        {
            try
            {
                var jsonResponse = await _abstractRequest.AbstractRequestAsync($"/patients/{userId}", string.Empty);
                return JsonSerializer.Deserialize<Patient>(jsonResponse);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return null;
        }

        public async Task<User?> LoginAsync (string email, string password)
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
                var user = JsonSerializer.Deserialize<User>(jsonResponse);

                if (user == null)
                {
                    Console.WriteLine("Failed to deserialize the response.");
                    return null;
                }
                // Fetch additional details based on the role
                return user.Role switch
                {
                    // e.g. if the user is a patient, fetch the patient details
                    "Patient" => await GetPatientByIdAsync(user.UserID),
                    "Caregiver" => await GetCaregiverByIdAsync(user.UserID),
                    _ => user
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return null;
        }

        public Task<Patient?> PostPatientAsync(string param)
        {
            throw new NotImplementedException();
        }
    }


}
