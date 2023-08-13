namespace Bookify.Web2.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]

    public class Governorate:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<Area> areas { get; set; } = new List<Area>();
    }
}
