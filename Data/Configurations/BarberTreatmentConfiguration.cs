using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZentrixBarber.Data.Models;
namespace ZentrixBarber.Data.Configurations
{
    public class BarberTreatmentConfiguration : IEntityTypeConfiguration<BarberTreatment>
    {
        public void Configure(EntityTypeBuilder<BarberTreatment> entity)
        {
            entity.HasKey(bt => new { bt.BarberId, bt.TreatmentId });


            entity.HasOne(bt => bt.Barber)
                .WithMany(b => b.BarberTreatments)
                .HasForeignKey(bt => bt.BarberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(bt => bt.Treatment)
                .WithMany(t => t.BarberTreatments)
                .HasForeignKey(bt => bt.TreatmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
