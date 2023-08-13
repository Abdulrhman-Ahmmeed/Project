using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class EditCopyViewModel
    {
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = Errors.InvalidRange)]
        [Remote("PreventDublicationEdit", null!, AdditionalFields = "Id", ErrorMessage = "Already there is Copy has the same name")]
        public int EditionNumber { get; set; }
    }
}
