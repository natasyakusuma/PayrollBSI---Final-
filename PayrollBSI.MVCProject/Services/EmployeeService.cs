using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Newtonsoft.Json;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace PayrollBSI.MVCProject.Services
{
    public class EmployeeService : IEmployee
    {
        private const string BaseUrl = "https://localhost:7050/api";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeService> _logger;


        public EmployeeService(HttpClient httpClient, IConfiguration configuration, ILogger<EmployeeService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }
        private string GetBaseUrl()
        {
            return _configuration["BaseUrl"] + "/Employee";
        }

        public async Task<string> ChangePassword(string username, string newPassword)
        {
            try
            {
                var json = JsonSerializer.Serialize(newPassword);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PutAsync($"{GetBaseUrl()}/Username", data);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorMessage = $"Failed to update employee. Status code: {httpResponse.StatusCode}";
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    errorMessage += $", Response: {errorContent}";

                    throw new HttpRequestException(errorMessage);
                }

                // Password changed successfully, return success message
                return "Password changed successfully!";
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                throw new ApplicationException("An error occurred while changing the password.", ex);
            }
        }



        public async Task<IEnumerable<EmployeeModel>> GetAll()
        {
            try
            {
                _logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
                var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/All");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve Employees data. HTTP status code: {0}", httpResponse.StatusCode);
                    return Enumerable.Empty<EmployeeModel>(); // Return an empty collection
                }
                var content = await httpResponse.Content.ReadAsStringAsync();

                // Deserialize content into collection of employees
                var employees = JsonSerializer.Deserialize<IEnumerable<EmployeeModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return employees;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving employees: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while retrieving employees. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }




        public async Task<IEnumerable<EmployeeModel>> GetAllActive()
        {
            try
            {
                _logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
                var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/Active");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve Employees data. HTTP status code: {0}", httpResponse.StatusCode);
                    return Enumerable.Empty<EmployeeModel>(); // Return an empty collection
                }

                var content = await httpResponse.Content.ReadAsStringAsync();

                // Deserialize content into collection of employees
                var employees = JsonSerializer.Deserialize<IEnumerable<EmployeeModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Successfully fetched {0} active employee", employees.Count());

                return employees;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving active employees: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while retrieving active employees. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }




        public async Task<EmployeeModel> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
                var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl}/{id}");
                httpResponse.EnsureSuccessStatusCode();

                var content = await httpResponse.Content.ReadAsStreamAsync();
                var employees = JsonSerializer.Deserialize<EmployeeModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return employees;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving active employees: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while retrieving active employees. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<string> GetByUsername(string username)
        {
            try
            {
                _logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
                var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/Username");
                httpResponse.EnsureSuccessStatusCode();


                var content = await httpResponse.Content.ReadAsStringAsync();
                var employees = JsonSerializer.Deserialize<EmployeeModel>
                    (content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return username;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving username employees: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while username active employees. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<LoginResponseModel> Login(string username, string password)
        {
            try
            {
                var loginData = new { username, password };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{GetBaseUrl()}/Login", jsonContent);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(responseData);

                return loginResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while tried to login: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while tried to login. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }


        public Task<IEnumerable<EmployeeModel>> Search(string employeeName, string positionName, string roleName)
        {
            throw new NotImplementedException();
        }



    }
}
