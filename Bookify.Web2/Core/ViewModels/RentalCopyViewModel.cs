namespace Bookify.Web2.Core.ViewModels
{
    public class RentalCopyViewModel
    {
        public BookCopiesModel? bookCopies { get; set; }
        public DateTime RentalDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ExtendOn { get; set; }
        public int DelayInDays 
        {
            get
            {
                var delay = 0;
                if (ReturnedDate.HasValue && ReturnedDate.Value>EndDate)
                     delay = (int)(ReturnedDate.Value - EndDate).TotalDays;
                else if (!ReturnedDate.HasValue && DateTime.Today>EndDate)
                    delay = (int)(DateTime.Today - EndDate).TotalDays;
                return delay;
            }
        }
    }
}
