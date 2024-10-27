using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json; // Add this line
using YourNamespace.Models;

namespace RequestManagerTest
{
    public class RequestManagerTests
    {
        private readonly Mock<AbstractRequest> _mockAbstractRequest;
        private readonly RequestManager _requestManager;

        public RequestManagerTests()
        {
            _mockAbstractRequest = new Mock<AbstractRequest>();
            _requestManager = new RequestManager(_mockAbstractRequest.Object);
        }

        [Fact]
        public async Task GetCaregiverByIdAsync_ReturnsCaregiver()
        {
            // Arrange
            int userId = 1;
            var expectedCaregiver = new Caregiver { UserID = userId, CaregiverID = "C1" };
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync($"/caregivers/{userId}", string.Empty))
                .ReturnsAsync(JsonSerializer.Serialize(expectedCaregiver));

            // Act
            var result = await _requestManager.GetCaregiverByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCaregiver.UserID, result.UserID);
            Assert.Equal(expectedCaregiver.CaregiverID, result.CaregiverID);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsPatient()
        {
            // Arrange
            int userId = 1;
            var expectedPatient = new Patient { UserID = userId, PatientID = 1 };
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync($"/patients/{userId}", string.Empty))
                .ReturnsAsync(JsonSerializer.Serialize(expectedPatient));

            // Act
            var result = await _requestManager.GetPatientByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPatient.UserID, result.UserID);
            Assert.Equal(expectedPatient.PatientID, result.PatientID);
        }

        [Fact]
        public async Task LoginAsync_ReturnsUserWithRolePatient()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password";
            var user = new User { UserID = 1, Role = "Patient" };
            var patient = new Patient { UserID = 1, PatientID = 1 };
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync("/login", It.IsAny<string>()))
                .ReturnsAsync(JsonSerializer.Serialize(user));
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync($"/patients/{user.UserID}", string.Empty))
                .ReturnsAsync(JsonSerializer.Serialize(patient));

            // Act
            var result = await _requestManager.LoginAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Patient>(result);
            Assert.Equal(patient.UserID, result.UserID);
            Assert.Equal(patient.PatientID, ((Patient)result).PatientID);
        }

        [Fact]
        public async Task LoginAsync_ReturnsUserWithRoleCaregiver()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password";
            var user = new User { UserID = 1, Role = "Caregiver" };
            var caregiver = new Caregiver { UserID = 1, CaregiverID = "C1" };
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync("/login", It.IsAny<string>()))
                .ReturnsAsync(JsonSerializer.Serialize(user));
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync($"/caregivers/{user.UserID}", string.Empty))
                .ReturnsAsync(JsonSerializer.Serialize(caregiver));

            // Act
            var result = await _requestManager.LoginAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Caregiver>(result);
            Assert.Equal(caregiver.UserID, result.UserID);
            Assert.Equal(caregiver.CaregiverID, ((Caregiver)result).CaregiverID);
        }

        [Fact]
        public async Task PostPatientAsync_ReturnsPatient()
        {
            // Arrange
            string method = "/patients";
            var expectedPatient = new Patient { UserID = 1, PatientID = 1 };
            _mockAbstractRequest.Setup(x => x.AbstractRequestAsync(method, string.Empty))
                .ReturnsAsync(JsonSerializer.Serialize(expectedPatient));

            // Act
            var result = await _requestManager.PostPatientAsync(method);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPatient.UserID, result.UserID);
            Assert.Equal(expectedPatient.PatientID, result.PatientID);
        }
    }
}
