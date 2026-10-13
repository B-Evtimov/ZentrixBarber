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
    }
}