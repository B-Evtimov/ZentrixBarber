using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ZentrixBarber.Common.EntityValidationConstants.Barber;

namespace ZentrixBarber.Data.Models
{
    public class Barber
    {
        [Key]

        [Comment("Barber identifier")]
        public int Id { get; set; }

        [Required]

        [MaxLength(FullNameMaxLength)]

        [Comment("Barber full name")]
        public string FullName { get; set; } = null!;

        [MaxLength(BioMaxLength)]

        [Comment("Short biography")]
        public string? Bio { get; set; }

        [MaxLength(ImageUrlMaxLength)]

        [Comment("Profile picture URL")]
        public string? ImageUrl { get; set; }

        [Comment("Years of professional experience")]
        public int YearsOfExperience { get; set; }

        [Comment("Whether the barber currently accepts appointments")]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<BarberTreatment> BarberTreatments { get; set; }
            = new HashSet<BarberTreatment>();

        public virtual ICollection<Appointment> Appointments { get; set; }
        = new HashSet<Appointment>();
    }





}
