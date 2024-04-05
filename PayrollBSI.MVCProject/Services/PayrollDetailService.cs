using System.Text.Json;
using Newtonsoft.Json;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PayrollBSI.MVCProject.Services
{
	public class PayrollDetailService : IPayrollDetails
	{
		private const string BaseUrl = "https://localhost:7050/api";
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<PayrollDetailService> _logger;


		public PayrollDetailService(HttpClient httpClient, IConfiguration configuration, ILogger<PayrollDetailService> logger)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
		}

		private string GetBaseUrl()
		{
			return _configuration["BaseUrl"] + "/PayrollDetails";
		}

		private string GetBaseUrlAttendance()
		{
			return _configuration["BaseUrl"] + "/Attendance";
		}

		public async Task<IEnumerable<PayrollDetailsModel>> GetAll()
		{
			try
			{
				_logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
				var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/All");
				httpResponse.EnsureSuccessStatusCode();

				var content = await httpResponse.Content.ReadAsStringAsync();
				var insertedPayrolls = JsonSerializer.Deserialize<List<PayrollDetailsModel>>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				return insertedPayrolls;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving payrolls: {Message}", ex.Message);
				throw new ApplicationException("Error occurred while retrieving payrolls. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}


		public async Task<PayrollDetailsModel> GetById(int id)
		{
			try
			{
				_logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
				var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/GetById/{id}");
				httpResponse.EnsureSuccessStatusCode();

				var content = await httpResponse.Content.ReadAsStringAsync();
				var payrollDetails = JsonSerializer.Deserialize<PayrollDetailsModel>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				return payrollDetails;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving payroll details: {Message}", ex.Message);
				throw new ApplicationException($"Error occurred while retrieving payroll details with ID {id}. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}

		public async Task<IEnumerable<AttendanceModel>> GetAllAttendances()
		{
			try
			{
				_logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrlAttendance());
				var httpResponse = await _httpClient.GetAsync($"{GetBaseUrlAttendance()}/All");
				httpResponse.EnsureSuccessStatusCode();

				var content = await httpResponse.Content.ReadAsStringAsync();
				var insertedAttendance = JsonSerializer.Deserialize<IEnumerable<AttendanceModel>>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				return insertedAttendance;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving payrolls: {Message}", ex.Message);
				throw new ApplicationException("Error occurred while retrieving payrolls. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}


		public async Task<bool> Insert(int employeeId, int attendanceId)
		{
			try
			{
				_logger.LogInformation("Base Url: {BaseUrl}", GetBaseUrl());
				var httpResponse = await _httpClient.GetAsync($"{GetBaseUrl()}/Insert?employeeId={employeeId}&attendanceId={attendanceId}");
				httpResponse.EnsureSuccessStatusCode();

				var content = await httpResponse.Content.ReadAsStringAsync();
				var payrollDetails = JsonSerializer.Deserialize<PayrollDetailsModel>(content, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
				return true;
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "Error occurred while retrieving payroll details: {Message}", ex.Message);
				throw new ApplicationException($"Error occurred while retrieving payroll details. See logs for details.", ex);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
				throw;
			}
		}


	}
}
