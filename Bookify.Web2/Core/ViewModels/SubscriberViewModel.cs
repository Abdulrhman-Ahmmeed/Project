namespace Bookify.Web2.Core.ViewModels
{
    public class SubscriberViewModel
    {

        public int Id { get; set; }
        public string? key { get; set; }

        public string? FullName { get; set; } 

        public DateTime BirthDate { get; set; }

        public string? NationalId { get; set; } 

        public string? MobileNumber { get; set; } 
        public bool? HasWhatsApp { get; set; }


        public string? Email { get; set; } 

        public string? ImageUrl { get; set; } 

        public string? ImageThumnabilUrl { get; set; } 

        public string? governorate { get; set; }

        public string? area { get; set; }

        public string? Address { get; set; } 
        public bool IsBlackedList { get; set; }
        public DateTime CreatedOn { get; set; }

        public IEnumerable<SubscribtionViewModel> subscribtions { get; set; } = new List<SubscribtionViewModel>();
        public IEnumerable<RentalViewModel> rentals { get; set; } = new List<RentalViewModel>();


    }
}
