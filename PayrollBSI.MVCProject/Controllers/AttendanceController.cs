using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;

namespace PayrollBSI.MVCProject.Controllers
{
	public class AttendanceController : Controller
	{

		private readonly IAttendance _attendanceService;
		private readonly ILogger<AttendanceController> _logger;

		public AttendanceController(IAttendance attendance, ILogger<AttendanceController> logger)
		{
			_attendanceService = attendance;
			_logger = logger;
		}


		public async Task<IActionResult> Index(int employeeId)
		{
			var allAttendances = await _attendanceService.GetAllAttendance();
			var attendancesFiltered = allAttendances.Where(a => a.EmployeeId == employeeId).ToList();
			return View(attendancesFiltered);
		}



		public async Task<IActionResult> Create()
		{
			try
			{
                if (HttpContext.Session.GetString("loginResponse") == null)
                {
                    return RedirectToAction("Login", "Employee");
                }
                var attendances = await _attendanceService.GetAllAttendance();

				// Create a HashSet to store unique employees
				var uniqueEmployees = new HashSet<SelectListItem>();

				// Iterate through the attendances and add unique employees to the HashSet
				foreach (var attendance in attendances)
				{
					var employeeId = attendance.EmployeeId;
					var employeeName = $"{attendance.Employee.FirstName} {attendance.Employee.LastName}";

					// Check if the employee ID already exists in the HashSet
					if (!uniqueEmployees.Any(e => e.Value == employeeId.ToString()))
					{
						// Add the employee to the HashSet if it doesn't exist
						uniqueEmployees.Add(new SelectListItem { Value = employeeId.ToString(), Text = employeeName });
					}
				}

				// Pass the unique employees to the view
				ViewBag.Attendances = uniqueEmployees;

				return View();
			}
			catch (Exception ex)
			{
				TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
				return View("Index");
			}
		}


		[HttpPost]
		public async Task<IActionResult> CreatePost(AttendanceModel model)
		{
			try
			{
				var attendance = await _attendanceService.Insert(model);
				if (attendance == null)
				{
					TempData["Message"] = @"<div class='alert alert-success'><strong>Success!</strong> Insert Position Success !</div>";
				}
			}
			catch (Exception ex)
			{
				TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
				return View("Index");
			}
			return RedirectToAction("Index", new { employeeId = model.EmployeeId });
		}

	}
}
