using EventPlatformProjectMVC.Application.Interfaces;
using EventPlatformProjectMVC.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


/*
 * For Organizer can view the events that has created
 * and Create new events
 */

namespace EventPlatformProjectMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork ;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var organizerId = _userManager.GetUserId(User);
            var events = await _unitOfWork.Events.GetAllAsync(
                include: x => x.Include(y => y.Organizer),
                filter: x => x.OrganizerId == organizerId);
            return View(events);
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
        [Authorize(Roles = $"{nameof(UserRole.Organizer)}")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            ViewBag.orgnizerId = user.Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event evenT)
        {
            evenT.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (evenT.PictureFile == null)
                    return View(evenT);

                using var memoryStream = new MemoryStream();
                await evenT.PictureFile.CopyToAsync(memoryStream);
                evenT.Picture = memoryStream.ToArray();

                await _unitOfWork.Events.AddAsync(evenT);
                int count = await _unitOfWork.SaveChangesAsync();

                return count > 0 ? RedirectToAction("Index") : View(evenT);
            }
            return View(evenT);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Organizer)}")]
        public async Task<IActionResult> Edit(int id)
        {
            var evenT = await _unitOfWork.Events.GetByIdAsync(id);
            return View(evenT);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Event evenT)
        {
            if (ModelState.IsValid)
            {
                if (evenT.PictureFile != null)
                {
                    using var memoryStream = new MemoryStream();
                    await evenT.PictureFile.CopyToAsync(memoryStream);
                    evenT.Picture = memoryStream.ToArray();
                }
                else
                {
                    var model = await _unitOfWork.Events.GetByIdAsync(evenT.Id);
                    evenT.Picture = model.Picture;
                }

                _unitOfWork.Events.Update(evenT);
                int count = await _unitOfWork.SaveChangesAsync();
                return count > 0 ? RedirectToAction("Index") : View(evenT);
            }
            return View(evenT);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Organizer)}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evenT = await _unitOfWork.Events.GetByIdAsync(id);
            _unitOfWork.Events.Delete(evenT);
            int count = await _unitOfWork.SaveChangesAsync();
            return count > 0 ? RedirectToAction("Index") : View(evenT);
        }

    }
}
