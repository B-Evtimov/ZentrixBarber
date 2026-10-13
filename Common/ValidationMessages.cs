namespace ZentrixBarber.Common
{
    public static class ValidationMessages
    {
        public const string RequiredMessage = "{0} is required.";
        public const string LengthMessage = "{0} must be between {2} and {1} characters.";
        public const string MaxLengthMessage = "{0} cannot be longer than {1} characters.";
        public const string InvalidEmailMessage = "Please enter a valid email address.";
        public const string InvalidPhoneMessage = "Please enter a valid phone number.";
        public const string InvalidSelectionMessage = "Please select a {0}.";
        public const string PastDateMessage = "Please choose a date and time.";
        public const string OutsideWorkingHoursMessage = "We are open from 09:00 to 20:00. The whole service must fit within working hours.";
        public const string TreatmentNotFoundMessage = "The selected service does not exist.";
        public const string BarberDoesNotOfferTreatmentMessage = "The selected barber does not offer this service.";
        public const string BarberBusyMessage = "The selected barber is already booked at this time. Please choose another time.";
    }
}