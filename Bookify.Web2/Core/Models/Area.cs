namespace Bookify.Web2.Core.Models
{
    //عشان امنع تكرار اسم المناطق في نفس المحاقظة
    [Index(nameof(Name),nameof(GovernorateId), IsUnique = true)]
    public class Area:BaseModel
    {

        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public int GovernorateId { get; set; }
        public Governorate? governorate { get; set; } 
    }
}
