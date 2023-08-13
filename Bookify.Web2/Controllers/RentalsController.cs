using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web2.Controllers
{
    [Authorize(Roles = AppRoles.Reception)]

    public class RentalsController : Controller
    {

        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDataProtector _dataProtector;

        public RentalsController(ApplicationDbContext context, IMapper mapper, IDataProtectionProvider dataprotector, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
            _dataProtector = dataprotector.CreateProtector("MySecureKey");

        }


        public IActionResult Create(string skey)
        {
            var subscriberId = int.Parse(_dataProtector.Unprotect(skey));
            var subscriber = _context.subscribers.Include(s => s.subscribtions)
                .Include(r => r.Rentals)
                .ThenInclude(rc => rc.RentalsCopies).FirstOrDefault(s => s.Id == subscriberId);

            if (subscriber is null)
                return NotFound();
            if (subscriber.IsBlackedList)
                return View("NotAllowedRental", Errors.NotAllowedRental);
            if (subscriber.subscribtions.Last().EndDate<DateTime.Now.AddDays((int)RentalsConfigurations.RentalDurations))
                return View("NotAllowedRental", Errors.InActive);

            var allowedRentalCopies = subscriber.Rentals.SelectMany(r => r.RentalsCopies).Count(r=>!r.ReturnedDate.HasValue);
            var AvailabeCopiesCount = ((int)RentalsConfigurations.MaxAllowedCopies) - allowedRentalCopies;
            if (AvailabeCopiesCount.Equals(0))
                return View("NotAllowedRental",Errors.MaxCopieshasBeenReached);
            var viewModel = new RentalFormViewModel
            {
                SubscriberKey = skey,
                MaxAllowedCopy=AvailabeCopiesCount

            };
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetBookCopiesDetils(SearchFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var copy = _context.bookCopies.
                Include(b => b.Book).
                SingleOrDefault(s => s.SerialNumber.ToString() == model.Value&&!s.IsDeleted&&!s.Book!.IsDeleted);

            if (copy is null)
                return NotFound(Errors.InvalidSerialNumber);
            if (!copy.IsAvailableForRental||!copy.Book!.IsAvailableForRental)
                return BadRequest(Errors.NotAvailableForRental);

            var viewModel = _mapper.Map<BookCopiesModel>(copy);

            return PartialView("_bookCopyDetails",viewModel);
        }
    }
}
