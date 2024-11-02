using Microsoft.EntityFrameworkCore;
using BlazorApp.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;

namespace BlazorApp.Data
{
    public class CallejoSystemDbContext : DbContext
    {
        public CallejoSystemDbContext(DbContextOptions<CallejoSystemDbContext> options) : base(options) { }

        public DbSet<CallejoIncUser> CallejoIncUsers { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite key for Guardian table
            modelBuilder.Entity<Guardian>()
                .HasKey(g => new { g.FkParent, g.FkChildren });

            // Define relationships
            modelBuilder.Entity<Guardian>()
                .HasOne(g => g.Parent)
                .WithMany(u => u.Guardians)
                .HasForeignKey(g => g.FkParent);

            modelBuilder.Entity<Guardian>()
                .HasOne(g => g.Child)
                .WithMany(c => c.Guardians)
                .HasForeignKey(g => g.FkChildren);

            modelBuilder.Entity<PhoneNumber>()
                .HasOne(p => p.User)
                .WithMany(u => u.PhoneNumbers)
                .HasForeignKey(p => p.FkUsers);

            modelBuilder.Entity<PhoneNumber>()
                .HasOne(p => p.PhoneNumberType)
                .WithMany(t => t.PhoneNumbers)
                .HasForeignKey(p => p.FkType);

            modelBuilder.Entity<CallejoIncUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.FkRole);

            // Configure additional entity settings if necessary
        }
    }
}
