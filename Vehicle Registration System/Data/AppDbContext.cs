using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vehicle_Registration_System.DTOs;
using Vehicle_Registration_System.Enums;
using Vehicle_Registration_System.Models;

namespace Vehicle_Registration_System.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Insurance> Insurances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Category)
                .HasConversion(
                    v => v.ToString(),
                    v => (Category)Enum.Parse(typeof(Category), v));

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Fuel)
                .HasConversion(
                    v => v.ToString(),
                    v => (Fuel)Enum.Parse(typeof(Fuel), v));

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Transmission)
                .HasConversion(
                    v => v.ToString(),
                    v => (Transmission)Enum.Parse(typeof(Transmission), v));

            modelBuilder.Entity<Owner>()
                .Property(o => o.Gender)
                .HasConversion(
                    v => v.ToString(),
                    v => (Gender)Enum.Parse(typeof(Gender), v));

            modelBuilder.Entity<Insurance>()
                .Property(i => i.InsuranceCompany)
                .HasConversion(
                    v => v.ToString(),
                    v => (InsuranceCompany)Enum.Parse(typeof(InsuranceCompany), v));
        }
    }
}
