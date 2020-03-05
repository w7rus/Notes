using System.Linq;
using Microsoft.EntityFrameworkCore;
using Notes.Logic.Models.Database;

namespace Notes.Logic.Data
{
    public class NotesWebAPIContext : DbContext
    {
        public NotesWebAPIContext(DbContextOptions<NotesWebAPIContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(i =>
            {
                i.HasIndex(u => u.Username)
                    .IsUnique();

                i.HasData(new User
                {
                    Id = 1,
                    Username = "public",
                    Password = null,
                    IsSystem = true
                });
            });

            builder.Entity<SharingProps>(i =>
            {
                i.HasOne(a => a.Note)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);

                i.HasOne(a => a.User)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade);

                i.HasKey(s => new
                {
                    s.NoteId,
                    s.UserId
                });
            });

            //builder.Entity<SharingProps>(i => { i.HasOne(x => x.Note).WithMany().OnDelete(DeleteBehavior.Restrict); })
            //    .HasData(new User
            //    {
            //        Id = 1,
            //        Username = "public",
            //        Password = null,
            //        IsSystem = true
            //    });

            //foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.NoAction;
            //}
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<SharingProps> Sharing { get; set; }
    }
}