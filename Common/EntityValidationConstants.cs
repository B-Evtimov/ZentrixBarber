namespace ZentrixBarber.Common
{
    public static class EntityValidationConstants
    {
        public static class Barber
        {
            public const int FullNameMinLength = 3;
            public const int FullNameMaxLength = 80;
            public const int BioMaxLength = 1000;
            public const int ImageUrlMaxLength = 2048;
            public const int YearsOfExperienceMin = 0;
            public const int YearsOfExperienceMax = 60;
        }
        
        public static class TreatmentCategory
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Treatment
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
            public const int DescriptionMaxLength = 1000;
            public const string PriceMin = "5.00";
            public const string PriceMax = "500.00";
            public const int DurationMinutesMin = 10;
            public const int DurationMinutesMax = 180;
        }

        public static class Appointment
        {
            public const int ClientNameMinLength = 2;
            public const int ClientNameMaxLength = 80;
            public const int ClientPhoneMinLength = 7;
            public const int ClientPhoneMaxLength = 20;
            public const int ClientEmailMaxLength = 100;
            public const int NotesMaxLength = 500;
        }
    }
}
