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

    public class Categories : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public Categories(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper= mapper; 
        
        }

        public IActionResult Index()
        {
            //TODO: use ViewModel
    
            var context = _context.categories.AsNoTracking().ToList();
            var ViewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(context);
            return View(ViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {

            return PartialView("_Create");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModelCategory model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var category = _mapper.Map<category>(model);
            category.CreatedOnByID= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.Add(category);
                _context.SaveChanges();
            var viewModel = _mapper.Map<CategoryViewModel>(category);
            return PartialView("_CategoryRow", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var category = _context.categories.Find(id);
            if (category is null) 
                return NotFound();
   
            var viewModel = _mapper.Map<EditViewModelCategory>(category);


            return PartialView("_Edit", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditViewModelCategory model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //from model to category
            var category = _mapper.Map<category>(model);
            if (category is null)
            {
                return NotFound();
            }
                //here i dont need for new Category so there is a different formal in Mapping declartion
                //special case in editing because we deal with existed element
                category= _mapper.Map(model,category);
            category.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            category.LastUpdated=DateTime.Now;
            _context.SaveChanges();
            /*            sending message using Tempdata becuasue it can save the message for two seasion
             *            after redirection will show the message
            */
            //from category to viewModel
            var viewModel = _mapper.Map<CategoryViewModel>(category);


            return PartialView("_CategoryRow", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var category= _context.categories.Find(id);
            if(category is null)
            return NotFound();
            //TRUE => FALSE
            //FALSE=>TRUE
            category.IsDeleted=!category.IsDeleted;
            category.LastUpdated=DateTime.Now;
            category.LastUpdatedById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();

            //OK?successful process   
            //category.LastUpdated.ToString() ?? TO SEND DATE TO PARAMETER WHICH INSIDE AJAX CALL WHEN WE CHANGE ANY SOMETHING 
            return Ok(category.LastUpdated.ToString());

        }


          public IActionResult PreventDublicationCreate(CreateViewModelCategory model)
        {
            var newCategory = _context.categories.Any(c=>c.Name==model.Name);

            return Json(!newCategory);
        }
         
        public IActionResult PreventDublicationEdit(EditViewModelCategory model)
        {
            var category = _context.categories.SingleOrDefault(c=>c.Name == model.Name);
            var IsAllowed= category is null || category.Id.Equals(model.Id);
            return Json(IsAllowed);
        }
    }
}
