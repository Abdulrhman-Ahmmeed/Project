using Bookify.Web2.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web2.Core.ViewModels
{
    public class UserEditViewModel
    {
        public string? Id { get; set; }

        [MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Full Name"),
            RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;



        [MaxLength(20, ErrorMessage = Errors.MaxLength), Display(Name = "Username"),
            RegularExpression(RegexPatterns.Username, ErrorMessage = Errors.InvalidUsername)]
        [Remote("AllowUsername", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public string UserName { get; set; } = null!;


        [MaxLength(100, ErrorMessage = Errors.MaxLength), EmailAddress]
        [Remote("AllowEmail", null!, AdditionalFields = "Id", ErrorMessage = Errors.DuplicatedBook)]
        public string Email { get; set; } = null!;

        //to make list of Roles
        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>();

        public IEnumerable<SelectListItem>? Roles { get; set; } = new List<SelectListItem>();
    }
}
