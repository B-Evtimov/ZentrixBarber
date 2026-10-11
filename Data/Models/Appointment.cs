using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZentrixBarber.Data.Models.Enums;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;

namespace ZentrixBarber.Data.Models
{
    public class Appointment
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ClientNameMaxLength)]
        
        public string ClientName { get; set; } = null!;

        [Required]
        [MaxLength(ClientPhoneMaxLength)]
        
        public string ClientPhone { get; set; } = null!;

        [MaxLength(ClientEmailMaxLength)]
        public string? ClientEmail { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [MaxLength(NotesMaxLength)]
        public string? Notes { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Comment("Barber identifier")]
        public int BarberId { get; set; }

        [ForeignKey(nameof(BarberId))]
        public virtual Barber Barber { get; set; } = null!;

        [Comment("Treatment identifier")]
        public int TreatmentId { get; set; }

        [ForeignKey(nameof(TreatmentId))]
        public virtual Treatment Treatment { get; set; } = null!;
    }
}
