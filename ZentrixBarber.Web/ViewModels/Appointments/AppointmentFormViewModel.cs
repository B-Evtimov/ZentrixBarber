using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ZentrixBarber.Common.EntityValidationConstants.Appointment;
using static ZentrixBarber.Common.ValidationMessages;

namespace ZentrixBarber.ViewModels.Appointments
{
    public class AppointmentFormViewModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(ClientNameMaxLength, MinimumLength = ClientNameMinLength, ErrorMessage = LengthMessage)]
        [Display(Name = "Full name")]
        public string ClientName { get; set; } = null!;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(ClientPhoneMaxLength, MinimumLength = ClientPhoneMinLength, ErrorMessage = LengthMessage)]
        [RegularExpression(ClientPhoneRegex, ErrorMessage = InvalidPhoneMessage)]
        [Display(Name = "Phone number")]
        public string ClientPhone { get; set; } = null!;

        [EmailAddress(ErrorMessage = InvalidEmailMessage)]
        [StringLength(ClientEmailMaxLength, ErrorMessage = MaxLengthMessage)]
        [Display(Name = "Email (optional)")]
        public string? ClientEmail { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [Display(Name = "Date and time")]
        public DateTime StartTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = InvalidSelectionMessage)]
        [Display(Name = "barber")]
        public int BarberId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = InvalidSelectionMessage)]
        [Display(Name = "service")]
        public int TreatmentId { get; set; }

        [StringLength(NotesMaxLength, ErrorMessage = MaxLengthMessage)]
        [Display(Name = "Notes (optional)")]
        public string? Notes { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Barbers { get; set; } = new List<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Treatments { get; set; } = new List<SelectListItem>();
    }
}