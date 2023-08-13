using Bookify.Web2.Enums;

namespace Bookify.Web2.Core.Models
{
    public class RentalCopy
    {
        public int RentalId { get; set; }
        public Rental? rental { get; set; }
        public int BookCopyID { get; set; }
        public BookCopy? BookCopy { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays((int)RentalsConfigurations.RentalDurations);
        public DateTime? ReturnedDate { get; set; }

        public DateTime? ExtendOn { get; set; }


    }
}
