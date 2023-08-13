namespace Bookify.Web2.Core.ViewModels
{
    public class CategoryViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; } 
        public DateTime? LastUpdated { get; set; }
    }
}
