namespace Bookify.Web2.Core.Models
{
    public class Subscribtions
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public Subscriber? subscriber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? CreatedOnByID { get; set; }
        public ApplicationUser? CreatedOnBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
