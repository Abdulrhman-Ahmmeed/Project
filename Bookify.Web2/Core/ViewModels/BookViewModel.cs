﻿namespace Bookify.Web2.Core.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Author { get; set; }

        public string Publisher { get; set; } = null!;

        public DateTime PublishingTime { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageThumnabilUrl { get; set; }

        public string Hall { get; set; } = null!;

        public bool IsAvailableForRental { get; set; }

        public string Description { get; set; } = null!;

        public IEnumerable<string> Categories { get; set; } = null!;
        public IEnumerable<BookCopiesModel> Copies { get; set; } = null!;

        public DateTime CreatedOn { get; set; } 
        public bool IsDeleted { get; set; }
    }
}
