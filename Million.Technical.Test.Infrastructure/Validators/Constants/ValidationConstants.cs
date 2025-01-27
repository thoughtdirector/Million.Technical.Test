namespace Million.Technical.Test.Infrastructure.Validators.Constants
{
    public static class ValidationConstants
    {
        public const string INTERNAL_CODE_REGEX_PATTERN = @"^[A-Za-z0-9\-_]+$";
        public const string PHOTO_NAME_REGEX_PATTERN = @"^.+\.(jpg|jpeg|png)$";
        public const decimal MIN_PRICE = 0.01m;
        public const int MIN_YEAR = 1800;
        public const int MAX_INTERNAL_CODE_LENGTH = 50;
        public const int MAX_PROPERTY_NAME_LENGTH = 100;
        public const int MIN_PROPERTY_NAME_LENGTH = 3;
        public const int MAX_ADDRESS_LENGTH = 250;
        public const int MIN_ADDRESS_LENGTH = 5;
        public const int MAX_IMAGE_SIZE_BYTES = 10 * 1024 * 1024;
    }
}