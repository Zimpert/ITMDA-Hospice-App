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
    /// <summary>
    /// Manages requests to the server.
    /// </summary>
    public class RequestManager : IRequestManager
    {
        private readonly ApiRequest _apiRequest;

        public RequestManager(ApiRequest apiRequest)
        {
            _apiRequest = apiRequest;
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
                var jsonResponse = await _apiRequest.SendRequestAsync("/userinfo", json);

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

        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                // Hash the password using SHA256
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    password = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }

                // Create an anonymous object with email and hashed password
                var jObject = new
                {
                    Email = email,
                    PasswordHash = password
                };

                // Log the email and hashed password for debugging
                Debug.WriteLine("Details:");
                Debug.WriteLine(jObject.Email);
                Debug.WriteLine(jObject.PasswordHash);

                // Serialize the object to JSON
                string json = JsonSerializer.Serialize(jObject);

                // Send the JSON to the server and get the response
                var jsonResponse = await _apiRequest.SendRequestAsync("/login", json);

                // Check if the response is empty
                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Console.WriteLine("No data returned from the server.");
                    return null;
                }

                // Log the response for debugging
                Console.WriteLine($"Response: {jsonResponse}");

                // Deserialize the JSON response to a User object
                var user = JsonSerializer.Deserialize<User>(jsonResponse);

                // Log the deserialization status for debugging
                Debug.WriteLine("Json Deseralize is over");

                // Check if deserialization was successful
                if (user == null)
                {
                    Debug.WriteLine("Failed to deserialize the response.");
                    return null;
                }

                // Log the user ID and token for debugging
                Debug.WriteLine($"UserID: {user.UserID}, Token: {user.Token}");

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

        public async Task Prelogin()
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
                await Shell.Current.GoToAsync("///HomePage");

            }
            return;
        }

        public async Task<bool> ValidateToken()
        {
            // Get the token from secure storage
            string? token = null;

            try
            {
                token = await SecureStorage.GetAsync("Token");
            }
            catch(Exception e)
            {
                Debug.WriteLine($"SecureStorage error: {e.Message}");
            }
            

            // Check if the token is missing
            if (string.IsNullOrEmpty(token))
            {
                Debug.WriteLine("Token missing.");
                return false;
            }

            // Log the token for debugging
            Debug.WriteLine($"Token: {token}");

            try
            {
                // Create an anonymous object with the token
                var jObject = new{ Token = token};

                // Serialize the object to JSON
                string json = JsonSerializer.Serialize(jObject);

                // Log the serialized JSON for debugging
                Debug.WriteLine($"Serialized JSON: {json}");

                // Send the JSON to the server and get the response
                var jsonResponse = await _apiRequest.SendRequestAsync("/prelogin", json);

                // Log the server response for debugging
                Debug.WriteLine($"Server response: {jsonResponse}");

                // Check if the response is empty
                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Debug.WriteLine("No response from server.");
                    return false;
                }

                // Log the JSON response for debugging
                Debug.WriteLine($"JSON Response: {jsonResponse}");

                try
                {
                    // Deserialize the JSON response to a dictionary
                    var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonResponse);

                    // Check if the deserialization was successful and contains the "Success" key
                    if (result != null && result.ContainsKey("Success"))
                    {
                        // Return the value of the "Success" key
                        return result["Success"].GetBoolean();
                    }
                    else
                    {
                        Debug.WriteLine("Response does not contain 'Success' key.");
                    }
                }
                catch (JsonException ex)
                {
                    // Handle JSON deserialization errors
                    Debug.WriteLine($"Deserialization error: {ex.Message}");
                }
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request errors
                Debug.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                Debug.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Return false if an error occurred
            return false;
        }

        public class Root
        {
            [JsonPropertyName("data")]
            public List<CaregiverShifts?> Data { get; set; }
        }

        public async Task<List<CaregiverShifts?>> GetCaregiverShiftsAsync(string userID, string token)
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
            var jsonResponse = await _apiRequest.SendRequestAsync("/shifts", json);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize the JSON response
            var root = JsonSerializer.Deserialize<Root>(jsonResponse, options);

            // Check if data is null
            if (root?.Data == null)
            {
                Debug.WriteLine("No data returned from the server.");
                return new List<CaregiverShifts?>();
            }

            // Iterate through each shift and parse ShiftStart and ShiftEnd
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

            // Return the list of CaregiverShifts objects
            return root.Data;
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



        public async Task<Dictionary<string, MedData?>> GetPatientMedicationsAsync(string token)
        {
            var jObject = new
            {
                Token = token
            };
            string json = JsonSerializer.Serialize(jObject);

            var jsonResponse = await _apiRequest.SendRequestAsync("/medicine", json);
            var result = JsonSerializer.Deserialize<MedicationRoot>(jsonResponse);

            return result.Data;
        }
    }


}
