using AutoMapper;
using Bookify.Web2.Core.Consts;
using Bookify.Web2.Core.Models;
using Bookify.Web2.Core.ViewModels;
using Bookify.Web2.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Bookify.Web2.Services;

namespace Bookify.Web2.Controllers
{  
    [Authorize(Roles =AppRoles.Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IEmailBodyBuilder _BodyBuilder;



        public UserController(UserManager<ApplicationUser> userManger, 
            IMapper mapper, RoleManager<IdentityRole> roleManager, IEmailSender emailSender,IEmailBodyBuilder BodyBuilder)
        {
            _userManger = userManger;
            _mapper = mapper;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _BodyBuilder = BodyBuilder;
        }


        public async Task <IActionResult> Index()
        {

            

            var users = await _userManger.Users.ToListAsync();
            var viewModel = _mapper.Map<IEnumerable<UserViewModel>>(users);


            return View(viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Create()
        {
            var viewModel = new UserFormViewModel
            {
                Roles = await _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync()
            };
           
            return PartialView("_Form",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Email = model.Email,
                CreatedOnByID = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };


           


            var result = await _userManger.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManger.AddToRolesAsync(user,model.SelectedRoles);

                var code = await _userManger.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var placeHolder = new Dictionary<string,string> ()
                {
                    {"imageUrl","https://res.cloudinary.com/devcreed/image/upload/v1668732314/icon-positive-vote-1_rdexez.svg" },
                    {"header",$"Hey {user.FullName}, thank for joining us!" },
                    {"body","Please Confirm your email" },
                    {"url",$"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                    {"linkTitle","Active Account" }
                
                };
                ///builder
                var body = _BodyBuilder.GetEmailBody(EmailTemplates.Email,placeHolder);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);

                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);

            }

            return BadRequest(string.Join(',',result.Errors.Select(e=>e.Description)));
        }


        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> ChangePassword(string Id)
        {
            var user = await _userManger.FindByIdAsync(Id);
            if (user is null)
                return NotFound();

            var viewModel = _mapper.Map<ChangePasswordViewModelUser>(user);


            return PartialView("_ChangePassword", viewModel);
        }

        /* [HttpPost]
         public async Task<IActionResult> ChangePassword(ChangePasswordViewModelUser model)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest();
             }

             var user = await _userManger.FindByIdAsync(model.Id);

             if (user is null)
             {
                 return NotFound();
             }

             // Verify the old password
             *//*            var oldPasswordCorrect = await _userManger.CheckPasswordAsync(user, model.OldPassword);
             *//*
             var oldPasswordCorrect = await _userManger.RemovePasswordAsync(user);




             var addPass = await _userManger.AddPasswordAsync(user,model.Password);



             // Return the updated user row as a partial view
             return PartialView("_ChangePassword", model);
         }*/


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModelUser model)
        {
            var user = await _userManger.FindByIdAsync(model.Id);

            if (user is null)
               return NotFound();
            
            var oldPasswordHash = user.PasswordHash;

            await _userManger.RemovePasswordAsync(user);

            var result = await _userManger.AddPasswordAsync(user,model.Password);

            if (result.Succeeded)
            {
                user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                user.LastUpdated = DateTime.Now;
                await _userManger.UpdateAsync(user);
                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow",viewModel);
            }
            user.PasswordHash = oldPasswordHash;
            await _userManger.UpdateAsync(user);

            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));

        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManger.FindByIdAsync(Id);
            if (user is null)
                return NotFound();

            var viewModel = _mapper.Map<UserFormViewModel>(user);

            // Set the previously assigned roles
            viewModel.SelectedRoles = await _userManger.GetRolesAsync(user);

            // Convert roles to SelectListItem objects
            viewModel.Roles =  await _roleManager.Roles
                .Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                })
                .ToListAsync();

            return PartialView("_Form", viewModel);
        }


        [HttpPost]
        [AjaxOnly]
        public async Task<IActionResult> Edit(UserFormViewModel Model)
        {
        
            var user = await _userManger.FindByIdAsync(Model.Id);
            if (user is null)
                return NotFound();


            // Update other properties
             user = _mapper.Map(Model,user);
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            user.LastUpdated = DateTime.Now;

            var result = await _userManger.UpdateAsync(user);
           
            if (result.Succeeded)
            {
                var CurrentRoles = await _userManger.GetRolesAsync(user);
                var rolesUpdate = !CurrentRoles.SequenceEqual(Model.SelectedRoles);

                if (rolesUpdate)
                {
                    await _userManger.RemoveFromRolesAsync(user, CurrentRoles);
                    await _userManger.AddToRolesAsync(user, Model.SelectedRoles);
                }
                await _userManger.UpdateSecurityStampAsync(user);

                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);
            }

            return BadRequest(string.Join(',', result.Errors.Select(e=>e.Description)));
        }


        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> ToggleStatus(string id)
        {
            
            var user = await _userManger.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            user.LastUpdated = DateTime.Now;
            await _userManger.UpdateAsync(user);
            if (user.IsDeleted)
            {
                await _userManger.UpdateSecurityStampAsync(user);

            }
            return Ok(user.LastUpdated.ToString());

        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Unlock(string id)
        {
            
            var user = await _userManger.FindByIdAsync(id);

            if (user == null)
                return NotFound();
            var isLock = await _userManger.IsLockedOutAsync(user);

            if (isLock)
            {
                await _userManger.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                await _userManger.SetLockoutEndDateAsync(user,DateTimeOffset.MaxValue);

            }

            return Ok();

        }


        public async Task <IActionResult> AllowUsername(UserFormViewModel model)
        {
            var user = await _userManger.FindByNameAsync(model.UserName);
            var IsAllowed = user is null || user.Id.Equals(model.Id);
            return Json(IsAllowed);
        } 
        public async Task <IActionResult> AllowEmail(UserFormViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            var IsAllowed = user is null || user.Id.Equals(model.Id);
            return Json(IsAllowed);
        }

    }
}
