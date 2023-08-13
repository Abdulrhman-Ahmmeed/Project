using AutoMapper;
using Bookify.Web2.Core.Consts;
using Bookify.Web2.Core.Models;
using Bookify.Web2.Core.ViewModels;
using Bookify.Web2.Filters;
using Bookify.Web2.Services;
using Bookify.Web2.setting;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace Bookify.Web2.Controllers
{
    public class Book : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly List<string> _AllowedExtensions = new () { ".jpg", ".jpeg", ".png" };
        private readonly int _MaxAllowedSize = 2097152;
        private readonly Cloudinary _cloudinary;
        public Book(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment,
                    IOptions<CloudinarySetting> cloudinary, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
            _imageService = imageService;
            Account account = new()
            { 
                Cloud = cloudinary.Value.CloudName,
                ApiKey = cloudinary.Value.APIKey,
                ApiSecret=cloudinary.Value.APISecret
            };
            _cloudinary = new Cloudinary(account);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBooks()
        {
            var skip =int.Parse(Request.Form["start"]);
            var pageSize =int.Parse(Request.Form["length"]);
            var searchValue = Request.Form["search[value]"];


            var sortColumnIndex = Request.Form["order[0][column]"];
            var SortCloumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];


            IQueryable<Core.Models.Book> books = _context.Books.Include(A=>A.Author)
                                                                .Include(b=>b.Categories)
                                                                .ThenInclude(c=>c.Category);
            if (!string.IsNullOrEmpty(searchValue))
            {
                books = books.Where(b => b.Title.Contains(searchValue)||b.Author!.Name.Contains(searchValue));

            }

            books = books.OrderBy($"{SortCloumn} {sortColumnDirection}");

            var data = books.Skip(skip).Take(pageSize).ToList();

            var mappingData = _mapper.Map<IEnumerable<BookViewModel>>(data);

            var recordsTotal = books.Count();
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data=mappingData };
            return Ok(jsonData);

        }

       

        public IActionResult Details(int id)
        {
            var book = _context.Books.
                Include(a => a.Author).
                Include(c => c.Copies).
                Include(c=>c.Categories).
                ThenInclude(n=>n.Category)
                .SingleOrDefault(b => b.Id == id);
            if (book is null)
                return NotFound();

            var viewModel = _mapper.Map<BookViewModel>(book);

            return View(viewModel);
        }
        public IActionResult create()
        {
            /* WITHOUT AUTOMAPPER
              var Authors = _context.authors.Where(a => !a.IsDeleted)
                                           .Select(x=>new SelectListItem { Value=x.Id.ToString(), Text=x.Name})
                                           .OrderBy(a => a.Text).ToList();*/
            var viewModel = RenderViewModel();
            return View("Form",viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
              return View("Form", RenderViewModel(model));
            var Book = _mapper.Map<Core.Models.Book>(model);
            if (model.Image is not null)
            {
                
                string imageName = $"{Guid.NewGuid()},{Path.GetExtension(model.Image.FileName)}";

                var (IsUploaded,errorMessage) = await _imageService.UploadAsync(model.Image, imageName, "/Images/books", IsThumbnail: true);

                if (IsUploaded)
                {
                    // to save url of image
                    // not just name of image
                    Book.ImageUrl = $"/Images/books/{imageName}";
                    // to save thumbUrl 
                    Book.ImageThumnabilUrl = $"/Images/books/thumb/{imageName}";
                }
                else
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form", RenderViewModel(model));
                }
                

              
                /* string imageName = $"{Guid.NewGuid()},{extension}";
                 using var stream = model.Image.OpenReadStream();
                 var imageParams = new ImageUploadParams()
                 {
                     File = new FileDescription(imageName, stream)
                 };
                 var result = await _cloudinary.UploadAsync(imageParams);

                 Book.ImageUrl = result.SecureUrl.ToString();*//*
                Book.ImageThumnabilUrl = getThumnabilUrl(Book.ImageUrl);
                Book.publicImageId = result.PublicId;*/
            }
            //we should make this mapp with our hand because in bookFormViewModel selctedList but in Book list<categories> 
            Book.CreatedOnByID= User.FindFirst(ClaimTypes.NameIdentifier)!.Value; 
            foreach (var categories in model.SelectedCategories)
                Book.Categories.Add(new BookCategory {CategoryId=categories });
            _context.Add(Book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details),new {id=Book.Id});
        }
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(c => c.Categories).SingleOrDefault(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            var model = _mapper.Map<BookFormViewModel>(book);
            var modelView = RenderViewModel(model);
            modelView.SelectedCategories = book.Categories.Select(x => x.CategoryId).ToList();
            return View("Form", modelView);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", RenderViewModel(model));

            //searching about this book is this book existed ??
            var book = _context.Books.Include(c => c.Categories).Include(B=>B.Copies).SingleOrDefault(b => b.Id == model.Id);

            if (book is null)
                return NotFound();
            //cloudinary
/*            string publicImageId = null;
*/
            // is there any pic in Model
            if (model.Image is not null)
            {
                //checking if the url not empty in book not in model
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {   //bring old path to work with it
                    _imageService.Delete(book.ImageUrl, book.ImageThumnabilUrl);
                    /*  await _cloudinary.DeleteResourcesAsync(book.publicImageId);
                    */
                }
                var imageName = $"{Guid.NewGuid()},{Path.GetExtension(model.Image.FileName)}";
                var (IsUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, "/Images/books", IsThumbnail: true);

                if (IsUploaded)
                {
                    // to save url of image
                    // not just name of image
                    model.ImageUrl = $"/Images/books/{imageName}";
                    // to save thumbUrl 
                    model.ImageThumnabilUrl = $"/Images/books/thumb/{imageName}";
                }
                else
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form", RenderViewModel(model));
                }


                // to save url of image
                // not just name of image
                model.ImageUrl = $"/Images/books/{imageName}";
                // to save thumbUrl 
                model.ImageThumnabilUrl = $"/Images/books/thumb/{imageName}";

            
                //cloudinary
                /*using var stream = model.Image.OpenReadStream();
                var imageParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageName, stream)
                };
                var result = await _cloudinary.UploadAsync(imageParams);
                model.ImageUrl = result.SecureUrl.ToString();
                publicImageId = result.PublicId;*/
            }
            //the user dosen't send any picture
            else if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                model.ImageUrl = book.ImageUrl;
                model.ImageThumnabilUrl = book.ImageThumnabilUrl;

            }
            //mapping in editing because there is no new instance
            book = _mapper.Map(model, book);
            book.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            book.LastUpdated = DateTime.Now;
            //cloudinary
       /*     book.ImageThumnabilUrl = getThumnabilUrl(model.ImageUrl!);
            book.publicImageId = publicImageId;*/
            //we should make this mapp with our hand because in bookFormViewModel selctedList but in Book list<categories> 
            // we maked this after mapping to have inistance we can work with if we didn't do it we will losse the list of category
            // because we have maked the proceed earlier then the rihght time eating after cooking
            foreach (var categories in model.SelectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = categories });
            if (!model.IsAvailableForRental)
            {
                foreach (var copy in book.Copies)
                {
                    copy.IsAvailableForRental = false;

                }

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Details),new {id=book.Id});
        }
        public IActionResult PreventDublication(BookFormViewModel model)
        {
            var Book = _context.Books.SingleOrDefault(B => B.Title == model.Title && B.AuthorId==model.AuthorId);
            var IsAllowed = Book is null || Book.Id.Equals(model.Id);
            return Json(IsAllowed);
        }
        public string getThumnabilUrl(string url)
        {
            var separetors = "image/upload/";
            var urlparts = url.Split(separetors);
            var thumnabil = $"{urlparts[0]}{separetors}c_thumb,w_200,g_face/{urlparts[1]}";
            return thumnabil ;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var book = _context.Books.Find(id);
            if (book is null)
                return NotFound();
            //TRUE => FALSE
            //FALSE=>TRUE
            book.IsDeleted = !book.IsDeleted;
            book.LastUpdated = DateTime.Now;
            book.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();

            //OK?successful process   
            //Author.LastUpdated.ToString() ?? TO SEND DATE TO PARAMETER WHICH INSIDE AJAX CALL
            //WHEN WE CHANGE ANY SOMETHING 
            return Ok();

        }
        private BookFormViewModel RenderViewModel (BookFormViewModel? model=null)
        {
            BookFormViewModel viewModel = model is null ? new BookFormViewModel() : model ;
            var Authors = _context.authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _context.categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToList();
            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(Authors);
            viewModel.categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            return (viewModel);
        }
    }
}
