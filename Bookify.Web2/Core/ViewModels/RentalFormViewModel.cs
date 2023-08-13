namespace Bookify.Web2.Core.ViewModels
{
    public class RentalFormViewModel
    {

        public string SubscriberKey { get; set; } = null!;

        public IList<int> SelectedCopies { get; set; } = new List<int>();

        public int? MaxAllowedCopy { get; set; }
    }
}
