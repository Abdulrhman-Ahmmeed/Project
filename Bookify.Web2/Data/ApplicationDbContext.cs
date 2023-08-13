using Bookify.Web2.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Author> authors { get; set; }
        public DbSet<Area> areas { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet <BookCopy> bookCopies { get; set; }
        public DbSet<category> categories { get; set; }
        public DbSet<Subscriber> subscribers { get; set; }
        public DbSet<Rental> rentals { get; set; }
        public DbSet<RentalCopy> rentalCopies { get; set; }
        public DbSet<Governorate> governorates { get; set; }
        public DbSet<Subscribtions> subscribtions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("SerialNumber", schema: "shared")
                   .StartsAt(1000001);

            builder.Entity<BookCopy>()
                      .Property(e => e.SerialNumber)
                      .HasDefaultValueSql("NEXT VALUE FOR shared.SerialNumber");

            //to make all my foreignKey restricted
            /* var cascadedFK = builder.Model.GetEntityTypes()
                 .SelectMany(t => t.GetForeignKeys())
                 .Where(x => x.DeleteBehavior == DeleteBehavior.Cascade && !x.IsOwnership);
             foreach (var fk in cascadedFK)
             {
                 fk.DeleteBehavior = DeleteBehavior.Restrict;

             }*/
            builder.Entity<BookCategory>().HasKey(e => new { e.BookId, e.CategoryId });
            builder.Entity<RentalCopy>().HasKey(e => new { e.RentalId, e.BookCopyID });

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    if (foreignKey.DeleteBehavior == DeleteBehavior.Cascade)
                        foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }



            base.OnModelCreating(builder);  
        }
    }
}