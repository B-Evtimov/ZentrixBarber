using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZentrixBarber.Data.Models;
namespace ZentrixBarber.Data.Configurations
{
    public class BarberConfiguration : IEntityTypeConfiguration<Barber>
    {
        public void Configure(EntityTypeBuilder<Barber> entity)
        {
            entity.HasData(SeedBarbers());
        }

        private static IEnumerable<Barber> SeedBarbers()
        {
            return new List<Barber>
            {
                new Barber
                {
                    Id = 1,
                    FullName = "Martin Atanasov",
                    Bio = "Experienced barber with a passion for classic cuts.",
                    YearsOfExperience = 10,
                    IsActive = true
                },

                new Barber
                {
                    Id = 2,
                    FullName = "George Donchev",
                    Bio = "Specializes in modern styles and beard grooming.",
                    YearsOfExperience = 8,
                    IsActive = true
                },

                new Barber
                {
                    Id = 3,
                    FullName = "Lazar Stoychev",
                    Bio = "Known for precision cuts and excellent customer service.",
                    YearsOfExperience = 12,
                    IsActive = true
                }
            };
        }
    }
}
