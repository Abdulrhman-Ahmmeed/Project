using AutoMapper;
using Bookify.Web2.Core.Consts;
using Bookify.Web2.Core.Models;
using Bookify.Web2.Core.ViewModels;
using Bookify.Web2.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bookify.Web2.Controllers
{
    [Authorize(Roles = AppRoles.Archive)]

    public class BookCopies : Controller
    {

        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public BookCopies(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create(int BookId)
        {   
            var book = _context.Books.Find(BookId);
            if (book is null)
                NotFound();

            /*If the Create action is called with the BookId parameter equal to 5,
             * the value of the BookId property in the viewModel will be 5. Therefore, 
             * when displaying the form in the partial view "_Create", the value of the hidden field BookId will be 5. 
             * And when the form is submitted, the value 5 will be sent to the server to identify the book for which a copy is being created.*/
            var viewModel = new CopiesFormViewModel 
            {
                BookId = BookId,
                IsAvailableForRental=book!.IsAvailableForRental,
                
            };
            
            return PartialView("_Create", viewModel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CopiesFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var book = _context.Books.Find(model.BookId);
            if (book is null)
                NotFound();


            BookCopy copies = new()
            {
                EditionNumber = model.EditionNumber,
                IsAvailableForRental = model.IsAvailableForRental,
                CreatedOnByID=User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                
            };
            book.Copies.Add(copies);
            _context.SaveChanges();
            /* The reason behind this order could be that the mapping is needed to generate a view model (BookCopiesModel)
             * that represents the newly created book copy with its associated data. By saving the changes to the database first, 
             * the copies object is assigned an identifier (such as an auto-generated primary key) that might be needed in the mapping process.*/
            var viewModel = _mapper.Map<BookCopiesModel>(copies);
            return PartialView("_BookCopies", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var copy = _context.bookCopies.Include(c=>c.Book).SingleOrDefault(c=>c.Id==id);
            if (copy is null)
                return NotFound();

            var viewModel = _mapper.Map<CopiesFormViewModel>(copy);
            viewModel.IsAvailableForRental = copy.Book!.IsAvailableForRental;

            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CopiesFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //from Model to Author
            var Copy = _context.bookCopies.Include(c => c.Book).SingleOrDefault(c => c.Id == model.Id);
            if (Copy is null)
            {
                return NotFound();
            }
            //here i dont need for new Copy so there is a different formal in Mapping declartion
            //special case in editing because we deal with existed element
            Copy.EditionNumber = model.EditionNumber;
            Copy.IsAvailableForRental = Copy.Book!.IsAvailableForRental && model.IsAvailableForRental ;
            Copy.LastUpdated = DateTime.Now;
            Copy.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();
            /*            sending message using Tempdata becuasue it can save the message for two seasion
             *            after redirection will show the message
            */
            //from COPY to BookCopiesModel
            var viewModel = _mapper.Map<BookCopiesModel>(Copy);

            ////////////////////////////////////////
            return PartialView("_BookCopies", viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var bookCopies = _context.bookCopies.Find(id);
            if (bookCopies is null)
                return NotFound();
            //TRUE => FALSE
            //FALSE=>TRUE
            bookCopies.IsDeleted = !bookCopies.IsDeleted;
            bookCopies.LastUpdated = DateTime.Now;
            bookCopies.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();

            //OK?successful process   
            //Author.LastUpdated.ToString() ?? TO SEND DATE TO PARAMETER WHICH INSIDE AJAX CALL
            //WHEN WE CHANGE ANY SOMETHING 
            return Ok();

        }
        public IActionResult PreventDublicationCreate(CopiesFormViewModel model)
        {
            var newCopies = _context.bookCopies.Any(A => A.EditionNumber == model.EditionNumber);

            return Json(!newCopies);
        }

       /* public IActionResult PreventDublicationEdit(EditViewModelAuthor model)
        {
            var Author = _context.authors.SingleOrDefault(A => A.Name == model.Name);
            var IsAllowed = Author is null || Author.Id.Equals(model.Id);
            return Json(IsAllowed);
        }*/

    }
}
