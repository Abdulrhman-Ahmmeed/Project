using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web2.Core.Models
{
    [Index(nameof(UserName),IsUnique =true)]
    [Index(nameof(Email),IsUnique =true)]
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public string? CreatedOnByID { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdated { get; set; }


    }
}
