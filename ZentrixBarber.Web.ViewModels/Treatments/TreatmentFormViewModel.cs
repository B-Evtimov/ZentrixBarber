using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ZentrixBarber.Common.EntityValidationConstants.Treatment;
using static ZentrixBarber.Common.ValidationMessages;

namespace ZentrixBarber.Web.ViewModels.Treatments
{
    public class TreatmentFormViewModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = LengthMessage)]
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength, ErrorMessage = MaxLengthMessage)]
        [Display(Name = "Description (optional)")]
        public string? Description { get; set; }

        [Range(typeof(decimal), PriceMin, PriceMax, ErrorMessage = RangeMessage)]
        [Display(Name = "Price (€)")]
        public decimal Price { get; set; }

        [Range(DurationMinutesMin, DurationMinutesMax, ErrorMessage = RangeMessage)]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = InvalidSelectionMessage)]
        [Display(Name = "category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}