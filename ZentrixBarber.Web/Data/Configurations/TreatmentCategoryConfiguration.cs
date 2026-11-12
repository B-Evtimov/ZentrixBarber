using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZentrixBarber.Data.Models;
namespace ZentrixBarber.Data.Configurations
{
    public class TreatmentCategoryConfiguration : IEntityTypeConfiguration<TreatmentCategory>
    {
        public void Configure(EntityTypeBuilder<TreatmentCategory> entity)
        {
            entity.HasData(SeedTreatmentCategories());
        }

        private static IEnumerable<TreatmentCategory> SeedTreatmentCategories()
        {
            return new List<TreatmentCategory>
            {
                new TreatmentCategory
                {
                    Id = 1,
                    Name = "Haircut",
                    Description = "Classic haircuts for all hair types."
                },

                new TreatmentCategory
                {
                    Id = 2,
                    Name = "Beard Grooming",
                    Description = "Professional beard trimming and styling."
                },

                new TreatmentCategory
                {
                    Id = 3,
                    Name = "Combo",
                    Description = "Combo treatment including haircut and beard grooming."
                }

            };
        }
    }
}
