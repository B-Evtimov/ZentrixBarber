using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZentrixBarber.Data.Models;

namespace ZentrixBarber.Data.Configurations
{
    public class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
    {
        public void Configure(EntityTypeBuilder<Treatment> entity)
        {
            entity.HasData(SeedTreatments());
        }

        private static IEnumerable<Treatment> SeedTreatments()
        {
            return new List<Treatment>
            {
                new Treatment
                {
                    Id = 1,
                    Name = "Classic Haircut",
                    Description = "A traditional haircut for men.",
                    Price = 15.00m,
                    DurationMinutes = 30,
                    CategoryId = 1
                },

                new Treatment
                {
                    Id = 2,
                    Name = "Skin Fade",
                    Description = "Precise fade down to the skin with a sharp line-up.",
                    Price = 18.00m,
                    DurationMinutes = 45,
                    CategoryId = 1
                },

                new Treatment
                {
                    Id = 3,
                    Name = "Kids Haircut",
                    Description = "Haircut for kids under 12.",
                    Price = 12.00m,
                    DurationMinutes = 30,
                    CategoryId = 1
                },

                new Treatment
                {
                    Id = 4,
                    Name = "Beard Trim & Shape",
                    Description = "Beard trim, shaping and hot towel finish.",
                    Price = 10.00m,
                    DurationMinutes = 20,
                    CategoryId = 2
                },

                new Treatment
                {
                    Id = 5,
                    Name = "Hot Towel Shave",
                    Description = "Traditional straight razor shave with hot towels.",
                    Price = 14.00m,
                    DurationMinutes = 30,
                    CategoryId = 2
                },
                new Treatment
                {
                    Id = 6,
                    Name = "Haircut & Beard",
                    Description = "Full haircut combined with beard trim and shaping.",
                    Price = 22.00m,
                    DurationMinutes = 60,
                    CategoryId = 3
                }
            };
        }
    }
}
