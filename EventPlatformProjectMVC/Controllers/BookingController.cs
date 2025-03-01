using Microsoft.AspNetCore.Mvc;
using EventPlatformProjectMVC.Application.Interfaces;
using EventPlatformProjectMVC.Domain;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;

namespace EventPlatformProjectMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManger;
        public BookingController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManger)
        {
            _unitOfWork = unitOfWork;
            _userManger = userManger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (ModelState.IsValid)
            {
                var uId = _userManger.GetUserId(User);
                var result = await _unitOfWork.Booking.GetAllAsync(
                    include: x => x.Include(y => y.User),
                    filter: x => x.UserID == uId
                );
                if(result == null)
                    return NoContent();
                return View(result);
            }
            return NotFound(ModelState);
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Admin)} , {nameof(UserRole.Organizer)}")]
        public async Task<IActionResult> Read(int id)
        {
            if(ModelState.IsValid)
            {
                var result = await _unitOfWork.Booking.GetByIdAsync(id);
                if (result == null)
                    return NoContent();
                return View(result);
            }
            return NotFound(ModelState);
        }


        [HttpGet , Authorize]
        public async Task<IActionResult> Details(int eventId)
        {
            if (ModelState.IsValid)
            {
                var eventU =  await _unitOfWork.Events.GetByIdAsync(eventId, include: x=> x.Include(y => y.Organizer));
                if(eventU == null)
                    return NoContent();
                else
                {
                    // save this event order id in session
                    HttpContext.Session.SetString("EventOrder", eventU.Id.ToString());
                    // return view with event details to active payment checkout
                    return View(eventU);
                }
            }
            return NoContent();
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            var domain = "https://localhost:44399/";
            var user = await _userManger.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index" , nameof(Booking));
            
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "Booking/OrderConfirmation",
                CancelUrl = domain + "Event/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email,
            };

            // get event order id from session
            var eventId = HttpContext.Session.GetString("EventOrder");
            if(eventId == null) return RedirectToAction("Index", nameof(Booking));
            var eventOrder = await _unitOfWork.Events.GetByIdAsync(int.Parse(eventId), include: x => x.Include(y => y.Organizer)); 
            if(eventOrder == null) return RedirectToAction("Index", nameof(Booking));

            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)eventOrder.Price /* * Quantity */,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = eventOrder.Title,
                        Description = eventOrder.Description,
                        //Images = eventOrder.Picture != null ? 
                        //                    new List<string> { Convert.ToBase64String(eventOrder.Picture) } : 
                        //                    new List<string>(),
                    },
                },
                Quantity = 1,
            };
            options.LineItems.Add(sessionLineItem);

            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Append("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            var service = new SessionService();
            if (TempData["Session"] == null)
                return View("Failed");
            var sessionId = TempData["Session"]!.ToString();
            if (string.IsNullOrEmpty(sessionId))
                return View("Failed");
            Session session = service.Get(sessionId);
            if (session.PaymentStatus.ToLower() == nameof(PaymentStatus.Paid).ToLower())
            {
                var userId = _userManger.GetUserId(User);
                if (userId == null)
                    return View("Failed");
                await _unitOfWork.Booking.AddAsync(new Booking
                {
                    UserID = userId,
                    TicketCount = 1,
                    TotalPrice = session.AmountTotal.HasValue ? (double)session.AmountTotal.Value : 0.0,
                    BookingDate = DateTime.Now,
                    PaymentStatus = nameof(PaymentStatus.Paid),
                    CreatedAt = DateTime.Now
                });
                return await _unitOfWork.SaveChangesAsync() > 0 ? View("Success") : View("Failed");
            }
            return View("Failed");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failed()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Attendee)} , {nameof(UserRole.Admin)}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _unitOfWork.Booking.GetByIdAsync(id);
            if(booking == null)
                return View(booking);
            _unitOfWork.Booking.Delete(booking);
            int count = await _unitOfWork.SaveChangesAsync();
            return count > 0 ? RedirectToAction("Index", nameof(Booking)) : View(booking);
        }
    }
}
