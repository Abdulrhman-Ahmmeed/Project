namespace Bookify.Web2.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }

        public string? CreatedOnByID { get; set; }
        public ApplicationUser? CreatedOnBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string? LastUpdatedById { get; set; }
        public ApplicationUser? LastUpdatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
