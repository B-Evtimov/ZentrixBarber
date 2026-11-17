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

            entity.HasData(
                new BarberTreatment { BarberId = 1, TreatmentId = 1 },
                new BarberTreatment { BarberId = 1, TreatmentId = 2 },
                new BarberTreatment { BarberId = 1, TreatmentId = 4 },
                new BarberTreatment { BarberId = 1, TreatmentId = 5 },
                new BarberTreatment { BarberId = 1, TreatmentId = 6 },
                new BarberTreatment { BarberId = 2, TreatmentId = 1 },
                new BarberTreatment { BarberId = 2, TreatmentId = 2 },
                new BarberTreatment { BarberId = 2, TreatmentId = 3 },
                new BarberTreatment { BarberId = 2, TreatmentId = 6 },
                new BarberTreatment { BarberId = 3, TreatmentId = 1 },
                new BarberTreatment { BarberId = 3, TreatmentId = 3 },
                new BarberTreatment { BarberId = 3, TreatmentId = 4 }
            );

        }
    }
}
