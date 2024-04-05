using Microsoft.AspNetCore.Mvc;
using PayrollBSI.MVCProject.Models;
using PayrollBSI.MVCProject.Services.Interface;

namespace PayrollBSI.MVCProject.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPosition _positionService;
        private readonly ILogger<PositionController> _logger;

        public PositionController(IPosition positionService, ILogger<PositionController> logger)
        {
            _positionService = positionService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("loginResponse") == null)
            {
                return RedirectToAction("Login", "Employee");
            }
            var positions = await _positionService.GetAllActivePositions();
            return View(positions);
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _positionService.DeletePosition(id);
                if (success)
                {
                    return Json(new { success = true }); //untuk nampilin biar langsung ke refresh harus return json
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Unable to delete position." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }


        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(PositionModel model)
        {
            try
            {
                var position = await _positionService.CreatePosition(model);
                if (position == null)
                {
                    TempData["Message"] = @"<div class='alert alert-success'><strong>Success!</strong>Insert Position Success !</div>";
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
                return View("Index");
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var model = await _positionService.GetPositionById(id);
            if (model == null)
            {
                TempData["message"] = @"<div class='alert alert-danger'>
                  <strong>Error!</strong> Position Not Found !</div>";
                return NotFound(); 
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PositionModel updateModel)
        {
            try
            {
                await _positionService.UpdatePosition(id, updateModel);
                TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong> Edit Data Position Success !</div>";
            }
            catch (Exception ex)
            {
                ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong> {ex.Message}</div>";
            }

			return RedirectToAction("Index");
		}


    }
}

