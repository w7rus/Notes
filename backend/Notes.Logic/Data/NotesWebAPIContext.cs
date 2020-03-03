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
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<SharingProps>()
                .HasKey(s => new
                {
                    s.NoteId,
                    s.UserId
                });

            builder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Username = "public",
                    Password = null
                });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<SharingProps> Sharing { get; set; }
    }
}