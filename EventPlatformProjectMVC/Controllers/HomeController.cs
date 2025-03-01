using System.Diagnostics;
using EventPlatformProjectMVC.Application.Interfaces;
using EventPlatformProjectMVC.Domain;
using EventPlatformProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlatformProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 4)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Events.GetAllAsync(pageNumber : pageNumber,pageSize:pageSize,include: q => q.Include(e => e.Organizer));
                if (result == null || !result.Any())
                    return View(new List<Event>());
                ViewData["PageNumber"] = pageNumber;
                return View(result);
            }
            return NotFound(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMore(int pageNumber)
        {
            // for Ajex
            int pageSize = 4;
            var result = await _unitOfWork.Events.GetAllAsync(pageNumber: pageNumber, pageSize: pageSize, include: q => q.Include(e => e.Organizer));

            if (result == null || !result.Any())
                return NoContent();
            return PartialView("_EventListPartial", result);
        }

        [HttpGet]
        //[Authorize(Roles = $"{nameof(UserRole.Admin)} , {nameof(UserRole.Organizer)}")]
        public async Task<IActionResult> Details(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.Events.GetByIdAsync(id);
                if (result == null)
                    return NoContent();
                return View(result);
            }
            return NotFound(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> Read(string title)
        {
            if (ModelState.IsValid)
            {
                var results = await _unitOfWork.Events.GetAllAsync();
                var filter = results.Where(x => x.Title == title).ToList();
                if (results == null || filter == null)
                    return NoContent();
                return View(filter);
            }
            return NotFound(ModelState);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
