using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class CopiesFormViewModel
    {

        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Is Available For Rental ?")]
        public bool IsAvailableForRental { get; set; }

        [Display(Name ="Edition Number"),Range(1,1000,ErrorMessage =Errors.InvalidRange)]
        public int EditionNumber { get; set; }

/*        public bool InputAvailableForRent { get; set; }
*/


    }
}
