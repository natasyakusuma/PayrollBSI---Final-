using Microsoft.AspNetCore.Http;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;

using System.Text;
using System.Text.Json;

namespace PayrollBSI.MVCProject.Services
{
    public class PositionServices : IPosition
    {
        private const string BaseUrl = "https://localhost:7050/api";
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PositionServices> _logger;

        public PositionServices(HttpClient httpClient, IConfiguration configuration, ILogger<PositionServices> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }
        private string GetBaseUrl()
        {
            return _configuration["BaseUrl"] + "/Position";
        }

        public async Task<PositionModel> CreatePosition(PositionModel position)
        {

            try
            {
                var json = JsonSerializer.Serialize(position);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync(GetBaseUrl() + "/Create", data);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot create Position");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                var category = JsonSerializer.Deserialize<PositionModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return category;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while creating positions: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while creating positions. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeletePosition(int id)
        {
            try
            {
                _logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
                var httpResponse = await _httpClient.DeleteAsync($"{GetBaseUrl()}/{id}");
                return httpResponse.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting positions: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while deleting positions. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }


        public async Task<IEnumerable<PositionModel>> GetAllPositions()
        {
            try
            {
                _logger.LogInformation("Base URL: {BaseUrl}", GetBaseUrl());

                var httpResponse = await _httpClient.GetAsync(GetBaseUrl());

                httpResponse.EnsureSuccessStatusCode(); // Throws HttpRequestException if not successful

                var content = await httpResponse.Content.ReadAsStringAsync();

                var positions = JsonSerializer.Deserialize<IEnumerable<PositionModel>>
                    (content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return positions;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving positions: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while retrieving positions. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }


        public async Task<PositionModel> GetPositionById(int id)
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/{id}");
                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Cannot retrieve Position");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                var position = JsonSerializer.Deserialize<PositionModel>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return position;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving positions: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while retrieving positions. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<PositionModel> UpdatePosition(int id, PositionModel position)
        {
            try
            {
                var json = JsonSerializer.Serialize(position);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PutAsync($"{GetBaseUrl()}/{id}", data);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorMessage = $"Failed to update position. Status code: {httpResponse.StatusCode}";
                    var errorContent = await httpResponse.Content.ReadAsStringAsync();
                    errorMessage += $", Response: {errorContent}";

                    throw new HttpRequestException(errorMessage);
                }

                var updatedPositionJson = await httpResponse.Content.ReadAsStringAsync();
                var updatedPosition = JsonSerializer.Deserialize<PositionModel>(updatedPositionJson);

                return updatedPosition;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while updating position data.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating position data.");
                throw;
            }
        }





        public async Task<IEnumerable<PositionModel>> GetAllActivePositions()
        {
            try
            {
                _logger.LogInformation("Fetching active positions...");

                // Make the HTTP GET request
                var httpResponse = await _httpClient.GetAsync(GetBaseUrl() + "/Active");

                // Check if the request was successful
                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve Position data. HTTP status code: {0}", httpResponse.StatusCode);
                    return Enumerable.Empty<PositionModel>(); // Return an empty collection
                }

                // Read the response content and deserialize it into PositionModel objects
                var content = await httpResponse.Content.ReadAsStringAsync();
                var positions = JsonSerializer.Deserialize<IEnumerable<PositionModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Successfully fetched {0} active positions", positions.Count());

                return positions;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while retrieving Position data.");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error occurred while deserializing Position data.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Position data.");
                return Enumerable.Empty<PositionModel>(); // Return an empty collection or handle the error as appropriate
            }
        }


    }
}
