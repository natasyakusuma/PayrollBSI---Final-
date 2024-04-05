using Microsoft.AspNetCore.Mvc;
using PayrollBSI.MVCProject.Services.Interface;


namespace PayrollBSI.MVCProject.Controllers
{
	public class PayrollDetailsController : Controller
	{
		private readonly IPayrollDetails _payrollDetailsService;
		private readonly ILogger<PayrollDetailsController> _logger;

		public PayrollDetailsController(IPayrollDetails payrollDetails, ILogger<PayrollDetailsController> logger)
		{
			_payrollDetailsService = payrollDetails;
			_logger = logger;
		}
		public async Task<IActionResult> Index(int employeeId)
		{
			if (HttpContext.Session.GetString("loginResponse") == null)
			{
				return RedirectToAction("Login", "Employee");
			}
			var payrollDetails = await _payrollDetailsService.GetAll();
			var payrollDetailsFiltered = payrollDetails.Where(a => a.EmployeeId == employeeId).ToList();
			return View(payrollDetailsFiltered);
		}

		public async Task<IActionResult> Create()
		{
			// Fetch employees data
			var employees = await _payrollDetailsService.GetAll();

			// Filter out duplicates by concatenated employee name
			var uniqueEmployees = employees.GroupBy(e => e.Employee.FirstName + " " + e.Employee.LastName)
										   .Select(g => g.First())
										   .ToList();

			var attandences = await _payrollDetailsService.GetAllAttendances();
			ViewBag.employeeList = uniqueEmployees;


			var uniqueAttendances = attandences.GroupBy(a => a.AttendanceId)
									.Select(g => g.LastOrDefault())
									.ToList();
			

			ViewBag.attendanceList = attandences;


			// Pass the initialized view model to the view
			return View();
		}


		[HttpPost("Insert")]
		public async Task<IActionResult> Insert(int employeeId, int attendanceId)
		{
			try
			{
				TempData["message"] = "<div class='alert alert-success'><strong>Success!</strong> Data inserted successfully.</div>";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				// If an error occurs, set an error message
				TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong> {ex.Message}</div>";
				return RedirectToAction("Index");
			}
		}
	}
}
