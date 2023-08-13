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

    public class Authors : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public Authors(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public IActionResult Index()
        {
            //TODO: use ViewModel

            var context = _context.authors.AsNoTracking().ToList();
            var ViewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(context);
            return View(ViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            /////////////////
            return PartialView("_Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModelAuthor model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Author = _mapper.Map<Author>(model);
            Author.CreatedOnByID = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.Add(Author);
            _context.SaveChanges();
            var viewModel = _mapper.Map<AuthorViewModel>(Author);


            /////////////
            return PartialView("_CategoryRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var Author = _context.authors.Find(id);
            if (Author is null)
                return NotFound();

            var viewModel = _mapper.Map<EditViewModelAuthor>(Author);
            

            return PartialView("_Edit", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditViewModelAuthor model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //from Model to Author
            var Author = _mapper.Map<Author>(model);
            if (Author is null)
            {
                return NotFound();
            }
            //here i dont need for new Author so there is a different formal in Mapping declartion
            //special case in editing because we deal with existed element
            Author = _mapper.Map(model, Author);
            Author.LastUpdated = DateTime.Now;
            Author.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value; 
            _context.SaveChanges();
            /*            sending message using Tempdata becuasue it can save the message for two seasion
             *            after redirection will show the message
            */
            //from category to viewModel
            var viewModel = _mapper.Map<AuthorViewModel>(Author);

            ////////////////////////////////////////
            return PartialView("_CategoryRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var Author = _context.authors.Find(id);
            if (Author is null)
                return NotFound();
            //TRUE => FALSE
            //FALSE=>TRUE
            Author.IsDeleted = !Author.IsDeleted;
            Author.LastUpdated = DateTime.Now;
            Author.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value; 
            _context.SaveChanges();

            //OK?successful process   
            //Author.LastUpdated.ToString() ?? TO SEND DATE TO PARAMETER WHICH INSIDE AJAX CALL
            //WHEN WE CHANGE ANY SOMETHING 
            return Ok(Author.LastUpdated.ToString());

        }


        public IActionResult PreventDublicationCreate(CreateViewModelAuthor model)
        {
            var newAuthor = _context.authors.Any(A => A.Name == model.Name);

            return Json(!newAuthor);
        }

        public IActionResult PreventDublicationEdit(EditViewModelAuthor model)
        {
            var Author = _context.authors.SingleOrDefault(A => A.Name == model.Name);
            var IsAllowed = Author is null || Author.Id.Equals(model.Id);
            return Json(IsAllowed);
        }
    }
}
