using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class CreateViewModelCategory
    {
        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        [Remote("PreventDublicationCreate", null, ErrorMessage = "Already there is item has the same name")]
        public string Name { get; set; } = null!;
    }
}
