using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;

namespace PayrollBSI.MVCProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployee employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("loginResponse") == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            var employees = await _employeeService.GetAll();
            return View(employees);
        }


		//public IActionResult GetUsername()
		//{
		//	// Fetch the username from the session
		//	var username = HttpContext.Session.GetString("Username");

		//	// Create a model with the username
		//	var model = new EmployeeModel
		//	{
		//		Username = username
		//	};

		//	// Pass the model to the view
		//	return View(model);
		//}

		public async Task<IActionResult> Login(string username, string password)
        {
            return View();
        }


		[HttpPost]
		public async Task<IActionResult> LoginPost(string username, string password)
		{
			// Perform authentication process and get login response
			var loginResponse = await _employeeService.Login(username, password);

			// Serialize the login response
			var loginResponseSerializer = JsonSerializer.Serialize(loginResponse);

			// Store the serialized login response in the session
			HttpContext.Session.SetString("loginResponse", loginResponseSerializer);

            //OPEN SESSION PAKE VAR

			// Redirect the user to another page after successful login
			return RedirectToAction("Index", "Home");
		}


		public async Task<IActionResult> Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect the user to the login page or any other appropriate page
            return RedirectToAction("Login", "Employee");
        }



    }
}
