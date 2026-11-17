using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZentrixBarber.Common.EntityValidationConstants.Treatment;

namespace ZentrixBarber.Data.Models
{
    public class Treatment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        
        [Precision(18, 2)]
        public decimal Price { get; set; }

       
        public int DurationMinutes { get; set; }

        [Comment("Treatment category identifier")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual TreatmentCategory Category { get; set; } = null!;

        public virtual ICollection<BarberTreatment> BarberTreatments { get; set; }
            = new HashSet<BarberTreatment>();


        public virtual ICollection<Appointment> Appointments { get; set; }
            = new HashSet<Appointment>();
    }
}
