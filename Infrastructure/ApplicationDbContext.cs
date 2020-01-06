using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Deck> Deck { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<AccessRights> AccessRights { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Analytics>()
                .HasKey(a => new { a.DeckId, a.RepeatDate });
            modelBuilder.Entity<AccessRights>()
                .HasKey(st => new { st.FromId, st.ToId });
        }
    }
}
