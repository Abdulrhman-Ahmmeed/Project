namespace Bookify.Web2.Core.Models
{
    public class BookCategory
    {

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public int CategoryId { get; set; }
        public category? Category { get; set; }


    }
}
