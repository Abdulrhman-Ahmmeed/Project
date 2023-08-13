namespace Bookify.Web2.Core.Models
{
    [Index(nameof(NationalId), IsUnique = true)]
    [Index(nameof(MobileNumber), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]

    public class Subscriber:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        [MaxLength(20)]
        public string NationalId { get; set; } = null!;

        [MaxLength(14)]
        public string MobileNumber { get; set; } = null!;
        public bool HasWhatsApp { get; set; }

        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [MaxLength(500)]
        public string ImageUrl { get; set; } = null!;

        [MaxLength(500)]
        public string ImageThumnabilUrl { get; set; } = null!;

        public int GovernorateId { get; set; }
        public Governorate? governorate { get; set; }

        public int areaId { get; set; }
        public Area? area { get; set; }

        public string Address { get; set; } = null!;

        public bool IsBlackedList { get; set; }
        public ICollection<Subscribtions> subscribtions { get; set; } = new List<Subscribtions>();
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();



    }
}
