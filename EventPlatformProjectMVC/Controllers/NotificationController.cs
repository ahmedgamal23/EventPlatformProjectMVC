using EventPlatformProjectMVC.Application.Interfaces;
using EventPlatformProjectMVC.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlatformProjectMVC.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork.Notifications.GetAllAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Create(Notification notification)
        {
            return View();
        }



    }
}
