using Microsoft.EntityFrameworkCore;
using System;


/*In summary, the BookFormViewModel class is used to represent the data needed for the book form view,
  including the collection of options for the author dropdown list.
  On the other hand, the Book class represents the entity that will be stored in the database
  and includes additional properties for maintaining relationships and storing related data.
 * The Author property in each class serves different purposes based on the context in which it is used*/


namespace Bookify.Web2.Core.Models
{
    [Index(nameof(Title),nameof(AuthorId), IsUnique = true)]
    public class Book:BaseModel
    {

        public int Id { get; set; }
        [MaxLength(500)]
        public string Title { get; set; } = null!;

        public int AuthorId { get; set; }
        public Author? Author { get; set; }
       
        [MaxLength(200)]
        public string Publisher { get; set; } = null!;

        public DateTime PublishingTime { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageThumnabilUrl { get; set; }
        public string? publicImageId { get; set; }

        [MaxLength(50)]
        public string Hall { get; set; } = null!;

        public bool IsAvailableForRental { get; set; }

        public string Description { get; set; } = null!;

        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();
        public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();
    }

}
