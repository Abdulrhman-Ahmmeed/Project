using Bookify.Web2.Core.Models;
using Bookify.Web2.Core.Consts;

using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web2.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        [Remote("PreventDublication", null!, AdditionalFields = "Id,AuthorId", ErrorMessage = Errors.DuplicatedBook)]
        public string Title { get; set; } = null!;

        [Display(Name ="Author")]
        [Remote("PreventDublication", null!, AdditionalFields = "Id,Title", ErrorMessage = Errors.DuplicatedBook)]

        public int AuthorId { get; set; }
        /*The SelectListItem class is typically used in ASP.NET MVC applications
          to represent options in a dropdown list or a select list.
          Each SelectListItem object typically has properties such as Text, Value, and Selected */
        public IEnumerable<SelectListItem>? Authors { get; set; }

        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        public string Publisher { get; set; } = null!;

        [Display(Name = "Publishing Date")]
        [AssertThat("PublishingTime <= Today()",ErrorMessage = Errors.MaxLength)]
        public DateTime PublishingTime { get; set; } = DateTime.Now;

        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagePublicID { get; set; }
        public string? ImageThumnabilUrl { get; set; }


        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        public string Hall { get; set; } = null!;

        [Display(Name = "Is Available For Rental ?")]
        public bool IsAvailableForRental { get; set; } 

        public string Description { get; set; } = null!;

        [Display(Name ="Categories")]
        public IList<int> SelectedCategories { get; set; } = new List<int>();

        public IEnumerable<SelectListItem>? categories { get; set; }


    }
}
