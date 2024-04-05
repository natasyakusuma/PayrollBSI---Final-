using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Newtonsoft.Json;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayrollBSI.MVCProject.Services
{
	public class AttendanceService : IAttendance
	{
		private const string BaseUrl = "https://localhost:7050/api";
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<AttendanceService> _logger;

		public AttendanceService(HttpClient httpClient, IConfiguration configuration, ILogger<AttendanceService> logger)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
		}

		private string GetBaseUrl()
		{
			return _configuration["BaseUrl"] + "/Attendance";
		}

		public async Task<AttendanceModel> Insert(AttendanceModel attendance)
		{
			try
			{
				var json = JsonSerializer.Serialize(attendance);
				var data = new StringContent(json, Encoding.UTF8, "application/json");
				var httpResponse = await _httpClient.PostAsync($"{GetBaseUrl()}/Insert", data);

				httpResponse.EnsureSuccessStatusCode(); // Ensure success status code

				var content = await httpResponse.Content.ReadAsStringAsync();
				var insertedAttendance = JsonSerializer.Deserialize<AttendanceModel>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				return insertedAttendance;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while inserting attendance: {Message}", ex.Message);
				throw new ApplicationException("Error occurred while inserting attendance. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}


		public async Task<IEnumerable<AttendanceModel>> GetAllAttendance()
		{
			try
			{
				_logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
				var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/All");
				if (httpResponse == null)
				{
					_logger.LogError("Failed to retrieve Attendances data. HTTP status code: {0}", httpResponse.StatusCode);
					return Enumerable.Empty<AttendanceModel>(); 
				}

				var content = await httpResponse.Content.ReadAsStringAsync();
				var attendances = JsonSerializer.Deserialize<IEnumerable<AttendanceModel>>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				return attendances;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving attendances: {Message}", ex.Message);
				throw new ApplicationException("Error occurred while retrieving attendances. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}

        public async Task<IEnumerable<AttendanceModel>> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
                var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/Employee");
                httpResponse.EnsureSuccessStatusCode();

                var content = await httpResponse.Content.ReadAsStringAsync();
                var attendances = JsonSerializer.Deserialize<IEnumerable<AttendanceModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return attendances;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving attendance: {Message}", ex.Message);
                throw new ApplicationException("Error occurred while retrieving active attendance. See logs for details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }


        public async Task<AttendanceModel> GetById(int id)
		{
			try
			{
				_logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
				var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/{id}");
				httpResponse.EnsureSuccessStatusCode();

				var content = await httpResponse.Content.ReadAsStringAsync();
				var attendance = JsonSerializer.Deserialize<AttendanceModel>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				return attendance;

			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving attendance: {Message}", ex.Message);
				throw new ApplicationException("Error occurred while retrieving active attendance. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}

		public async Task<AttendanceModel> Update(int id, AttendanceModel attendanceModel)
		{
			try
			{
				var json = JsonSerializer.Serialize(attendanceModel);
				var data = new StringContent(json, Encoding.UTF8, "application/json");
				var httpResponse = await _httpClient.PutAsync($"{GetBaseUrl()}/{id}", data);

				if (!httpResponse.IsSuccessStatusCode)
				{
					var errorMessage = $"Failed to update position. Status code: {httpResponse.StatusCode}";
					var errorContent = await httpResponse.Content.ReadAsStringAsync();
					errorMessage += $", Response: {errorContent}";

					throw new HttpRequestException(errorMessage);
				}

				var updateAttendanceJson = await httpResponse.Content.ReadAsStringAsync();
				var updateAttendance = JsonSerializer.Deserialize<AttendanceModel>(updateAttendanceJson);

				return updateAttendance;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while updat e: {Message}", ex.Message);
				throw new ApplicationException("Error occurred while retrieving positions. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}
	}
}
