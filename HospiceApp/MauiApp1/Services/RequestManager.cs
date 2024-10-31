using MauiApp1.Classes;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using MauiApp1.ViewModels;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics;
using System.Text;
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

        public async Task<User?> GetUserDataAsync(string userID, string token)
        {
            try
            {
                // Create an anonymous object with userID and token
                var jObject = new
                {
                    UserID = userID,
                    Token = token
                };

                // Serialize the object to JSON
                string json = JsonSerializer.Serialize(jObject);

                // Send the JSON to the server and get the response
                var jsonResponse = await _abstractRequest.AbstractRequestAsync("/userinfo", json);

                // Deserialize the JSON response to a User object
                var user = JsonSerializer.Deserialize<User>(jsonResponse);

                // Check if deserialization was successful
                if (user == null)
                {
                    Console.WriteLine("Failed to deserialize the response.");
                    return null;
                }

                // Return the user object
                return user;
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request errors
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Return null if an error occurred
            return null;
        }

        public async Task<User?> LoginAsync (string email, string password)
        {
            
            try
            {
                // Hash the password using SHA256
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    password = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
                var jObject = new
                {
                    Email = email,
                    PasswordHash = password
                };
                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _abstractRequest.AbstractRequestAsync("/login", json);
                var user = JsonSerializer.Deserialize<User>(jsonResponse);

                if (user == null)
                {
                    Console.WriteLine("Failed to deserialize the response.");
                    return null;
                }
                Debug.WriteLine(user.UserID);
                return user;
                
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

        public async Task<bool> ValidateToken()
        {
            var token = await SecureStorage.GetAsync("Token");
            if (string.IsNullOrEmpty(token))
            {
                return false; // token missing 
            }

            try
            {
                var jObject = new
                {
                    Token = token
                };
                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _abstractRequest.AbstractRequestAsync("/prelogin", json);
                var result = JsonSerializer.Deserialize<bool>(jsonResponse);

                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return false;

        }
    }


}
