
using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class EditViewModelCategory
    {
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        [Remote("PreventDublicationEdit", null!, AdditionalFields = "Id", ErrorMessage = "Already there is item has the same name")]
        public string Name { get; set; } = null!;
    }
}
