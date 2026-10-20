using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZentrixBarber.Data.Models;

namespace ZentrixBarber.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public virtual DbSet<Barber> Barbers { get; set; } = null!;

        public virtual DbSet<TreatmentCategory> TreatmentCategories { get; set; } = null!;

        public virtual DbSet<Treatment> Treatments { get; set; } = null!;

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;

        public virtual DbSet<BarberTreatment> BarberTreatments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}