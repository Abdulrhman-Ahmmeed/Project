namespace Bookify.Web2.Core.ViewModels
{
    public class AuthorViewModel
    {
            public int Id { get; set; }
            [MaxLength(100)]
            public string Name { get; set; } = null!;
            public DateTime CreatedOn { get; set; } = DateTime.Now;
            public DateTime LastUpdated { get; set; }
            public bool IsDeleted { get; set; }

    }
}
