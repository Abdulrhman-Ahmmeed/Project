﻿using Microsoft.EntityFrameworkCore;

namespace Bookify.Web2.Core.Models
{
    [Index(nameof(Name), IsUnique =true)]
    public class category:BaseModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();

    }
}
