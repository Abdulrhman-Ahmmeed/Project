namespace Bookify.Web2.Core.ViewModels
{
    public class RentalViewModel
    {
        public int Id { get; set; }
        public SubscriberViewModel? subscriber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool PenaltyPaid { get; set; }
        public IEnumerable<RentalCopyViewModel> RentalsCopies { get; set; } = new List<RentalCopyViewModel>();

        public int TotalDelayInDays 
        {
            get 
            {
                return RentalsCopies.Sum(C => C.DelayInDays);
            }
        }
        public int NumberOfCopies 
        {
            get
            {
                return RentalsCopies.Count();
            }
            
        }
    }
}
