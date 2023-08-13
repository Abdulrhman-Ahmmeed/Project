namespace Bookify.Web2.Core.Models
{
    public class Rental:BaseModel
    {
        public int Id { get; set; }
        public int SubscriberID { get; set; }
        public Subscriber? subscriber { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public bool PenaltyPaid { get; set; }
        public ICollection<RentalCopy> RentalsCopies { get; set; } = new List<RentalCopy>();

    }
}
