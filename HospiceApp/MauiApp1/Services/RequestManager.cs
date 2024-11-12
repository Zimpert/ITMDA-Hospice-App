using MauiApp1.Classes;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using MauiApp1.ViewModels;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MauiApp1.Services
{
    public class RequestManager : IRequestManager
    {
        private readonly ApiRequest _apiRequest;

        public RequestManager(ApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
        }

        /// <summary>
        /// Retrieves user data using the provided userID and token.
        /// </summary>
        /// <param name="userID">The user ID of the user.</param>
        /// <param name="token">The token used for authentication.</param>
        /// <returns>A <see cref="User"/> object if the request is successful, or null if an error occurs.</returns>
        /// <exception cref="HttpRequestException">Thrown when there is an error sending the HTTP request.</exception>
        /// <exception cref="JsonException">Thrown when there is an error processing the JSON response.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<User?> GetUserDataAsync(string userID, string token)
        {
            try
            {
                //anonymous object with userID and token
                var jObject = new
                {
                    UserID = userID,
                    Token = token
                };

                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _apiRequest.SendRequestAsync("/userinfo", json);

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Console.WriteLine("No data returned from the server.");
                    await Application.Current.MainPage.DisplayAlert("Error", "No user data found.", "OK");
                    return null;
                }

                var user = JsonSerializer.Deserialize<User>(jsonResponse);

                if (user == null)
                {
                    Console.WriteLine("Failed to deserialize the response.");
                    return null;
                }

                return user;
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP errors
                Console.WriteLine($"Request error: {e.Message}");
                Debug.WriteLine($"Request error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Network error. Please try again.", "OK");
            }
            catch (JsonException e)
            {
                // Handle JSON serialization/deserialization errors
                Console.WriteLine($"JSON error: {e.Message}");
                Debug.WriteLine($"JSON error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Data processing error. Please try again.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Debug.WriteLine($"Unexpected error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }

            // null if error
            return null;
        }


        /// <summary>
        /// Authenticates a user using the provided email and password.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A <see cref="User"/> object if authentication is successful, or null if authentication fails.</returns>
        /// <exception cref="HttpRequestException">Thrown when there is an error sending the HTTP request.</exception>
        /// <exception cref="JsonException">Thrown when there is an error processing the JSON response.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                // Hash password using SHA256
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    password = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }

                //anonymous object with email and hashed password
                var jObject = new
                {
                    Email = email,
                    PasswordHash = password
                };

                //log for debug 
                Console.WriteLine("Details:");
                Console.WriteLine(jObject.Email);
                Console.WriteLine(jObject.PasswordHash);

                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _apiRequest.SendRequestAsync("/login", json);

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Console.WriteLine("No data returned from the server.");
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid login details.", "OK");
                    return null;
                }

                // Log for debug
                Console.WriteLine($"Response: {jsonResponse}");

                var user = JsonSerializer.Deserialize<User>(jsonResponse);

                Console.WriteLine("Json Deserialization is over");

                if (user == null)
                {
                    Console.WriteLine("Failed to deserialize the response.");
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid login details.", "OK");
                    return null;
                }

                if (user.UserID == null || user.Token == null)
                {
                    Console.WriteLine("User data is incomplete.");
                    await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid login details.", "OK");
                    return null;
                }

                // Log userID and token for debug
                Console.WriteLine($"UserID: {user.UserID}, Token: {user.Token}");

                return user;
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request errors
                Console.WriteLine($"Request error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Network error. Please try again.", "OK");
            }
            catch (JsonException e)
            {
                // Handle JSON serialization/deserialization errors
                Console.WriteLine($"JSON error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Data processing error. Please try again.", "OK");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Login Failed", "An unexpected error occurred. Please try again.", "OK");
            }

            return null;
        }


        public async void Prelogin()
        {
            string token = null;

            token = await SecureStorage.GetAsync("Token");
            if (token == null)
            {
                return;
            }
            var jsonObj = new {Token =  token};
            string jsonSerial = JsonSerializer.Serialize(jsonObj);
            // otherwise we havea token so send it to the request
            var response = await _apiRequest.SendRequestAsync("/prelogin", jsonSerial);
            var jsonDocument = JsonDocument.Parse(response);

            // Extract the "Success" value as a boolean
            bool isSuccess = jsonDocument.RootElement.GetProperty("Success").GetBoolean();
            if (isSuccess)
            {
                await Shell.Current.GoToAsync("//HomePage");

            }
            return;
        }


        public class Root
        {
            [JsonPropertyName("data")]
            public List<CaregiverShifts?> Data { get; set; }
        }


        /// <summary>
        /// Retrieves the caregiver shifts for a user using the provided userID and token.
        /// </summary>
        /// <param name="userID">The user ID of the caregiver.</param>
        /// <param name="token">The token used for authentication.</param>
        /// <returns>A list of caregiver shifts, or an empty list if an error occurs.</returns>
        public async Task<List<CaregiverShifts?>> GetCaregiverShiftsAsync(string userID, string token)
        {
            try
            {
                //anonymous object with userID and token
                var jObject = new
                {
                    UserID = userID,
                    Token = token
                };


                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _apiRequest.SendRequestAsync("/shifts", json);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var root = JsonSerializer.Deserialize<Root>(jsonResponse, options);

                if (root?.Data == null)
                {
                    Debug.WriteLine("No data returned from the server.");
                    return new List<CaregiverShifts?>();
                }

                foreach (var shift in root.Data)
                {
                    if (DateTime.TryParse(shift?.ShiftStart, out var shiftStart))
                    {
                        Debug.WriteLine($"Shift Start: {shiftStart}");
                    }
                    if (DateTime.TryParse(shift?.ShiftEnd, out var shiftEnd))
                    {
                        Debug.WriteLine($"Shift End: {shiftEnd}");
                    }
                }

                // Return CaregiverShifts objects list
                return root.Data;
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP errors
                Console.WriteLine($"Request error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Network error. Please try again.", "OK");
            }
            catch (JsonException e)
            {
                // Handle JSON serialization/deserialization errors
                Console.WriteLine($"JSON error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Data processing error. Please try again.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
            return new List<CaregiverShifts?>();
        }


        public class MedData
        {

            [JsonPropertyName("PatientInfo")]
            public PatientInfo PatientInfo { get; set; }

            [JsonPropertyName("Medication")]
            public List<Medication> Medication { get; set; }
        }


        public class MedicationRoot
        {
            [JsonPropertyName("data")]
            public Dictionary<string, MedData> Data { get; set; }
        }


        /// <summary>
        /// Retrieves the medication data for a patient using the provided token.
        /// </summary>
        /// <param name="token">The token used for authentication.</param>
        /// <returns>A dictionary containing medication data for the patient, or an empty dictionary if an error occurs.</returns>
        public async Task<Dictionary<string, MedData?>> GetPatientMedicationsAsync(string token)
        {
            try
            {
                var jObject = new
                {
                    Token = token
                };
                string json = JsonSerializer.Serialize(jObject);

                var jsonResponse = await _apiRequest.SendRequestAsync("/medicine", json);

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Console.WriteLine("No data returned from the server.");
                    return new Dictionary<string, MedData?>();
                }

                var result = JsonSerializer.Deserialize<MedicationRoot>(jsonResponse);

                // Check if the deserialization was successful and if data is null
                if (result?.Data == null || result.Data.Count == 0)
                {
                    Console.WriteLine("No medication data found.");
                    await Application.Current.MainPage.DisplayAlert("Error", "No medication data found.", "OK");
                    return new Dictionary<string, MedData?>();
                }

                return result.Data;
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request errors
                Console.WriteLine($"Request error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Network error. Please try again.", "OK");
            }
            catch (JsonException e)
            {
                // Handle JSON serialization/deserialization errors
                Console.WriteLine($"JSON error: {e.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Data processing error. Please try again.", "OK");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }

            // Return an empty dictionary if an error occurred
            return new Dictionary<string, MedData?>();
        }
    }


}
