using Bookify.Web2.Core.Consts;

namespace Bookify.Web2.Core.ViewModels
{
    public class CreateViewModelAuthor
    {
        [MaxLength(200, ErrorMessage = Errors.MaxLength),RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]

        [Remote("PreventDublicationCreate", null, ErrorMessage = Errors.Duplicated)]
        public string Name { get; set; } = null!;

    }
}
