using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class ChangePasswordViewModelUser
    {

        public string? Id { get; set; }

       /* [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; } = null!;

*/
        [Required]
        [StringLength(40, ErrorMessage = Errors.MaxLength, MinimumLength = 6), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password"), Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
        public string ConfirmPassword { get; set; } = null!;

    }
}
