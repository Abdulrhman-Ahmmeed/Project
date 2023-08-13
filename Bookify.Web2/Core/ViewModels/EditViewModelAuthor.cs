using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class EditViewModelAuthor
    {
        public int Id { get; set; }

        [MaxLength(200, ErrorMessage = Errors.MaxLength),
            RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]

        [Remote("PreventDublicationEdit", null!, AdditionalFields = "Id", ErrorMessage = "Already there is Author has the same name")]
        public string Name { get; set; } = null!;

    }
}
