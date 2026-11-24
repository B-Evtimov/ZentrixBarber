using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ZentrixBarber.Common.EntityValidationConstants.Barber;
using static ZentrixBarber.Common.ValidationMessages;

namespace ZentrixBarber.Web.ViewModels.Barbers
{
    public class BarberFormViewModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength, ErrorMessage = LengthMessage)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = null!;

        [StringLength(BioMaxLength, ErrorMessage = MaxLengthMessage)]
        [Display(Name = "Biography (optional)")]
        public string? Bio { get; set; }

        [Url(ErrorMessage = InvalidUrlMessage)]
        [StringLength(ImageUrlMaxLength, ErrorMessage = MaxLengthMessage)]
        [Display(Name = "Image URL (optional)")]
        public string? ImageUrl { get; set; }

        [Range(YearsOfExperienceMin, YearsOfExperienceMax, ErrorMessage = RangeMessage)]
        [Display(Name = "Years of experience")]
        public int YearsOfExperience { get; set; }

        [Display(Name = "Services")]
        public List<int> SelectedTreatmentIds { get; set; } = new List<int>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Treatments { get; set; } = new List<SelectListItem>();
    }
}