using System.ComponentModel.DataAnnotations;
using static ZentrixBarber.Common.EntityValidationConstants;
using static ZentrixBarber.Common.EntityValidationConstants.TreatmentCategory;

namespace ZentrixBarber.Data.Models
{
    public class TreatmentCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Treatment> Treatments { get; set; } = new HashSet<Treatment>();
    }
}
