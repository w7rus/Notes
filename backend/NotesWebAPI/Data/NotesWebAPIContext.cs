using Microsoft.EntityFrameworkCore;
using NotesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesWebAPI.Data
{
    public class NotesWebAPIContext : DbContext
    {
        public NotesWebAPIContext(DbContextOptions<NotesWebAPIContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Models.Database.User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }

        public DbSet<Models.Database.User> Users { get; set; }
        public DbSet<Models.Database.Note> Notes { get; set; }
    }
}