using System.Globalization;
using System.Text.RegularExpressions;

namespace Library.Presentation.Validation
{
    internal enum FieldKind
    {
        Name,
        Patronymic,
        Surname,
        Birthday,
        PhoneNumber,
        Address
    }

    internal readonly record struct ValidationResult(bool IsValid, string? Error)
    {
        public static ValidationResult Ok() => new(true, null);
        public static ValidationResult Fail(string error) => new(false, error);
    }

    internal static class InputValidator
    {
        private const string NamePattern = @"^[A-Za-zА-Яа-яЁё][A-Za-zА-Яа-яЁё' -]*[A-Za-zА-Яа-яЁё]?$";
        private const string SurnamePattern = @"^[A-Za-zА-Яа-яЁё][A-Za-zА-Яа-яЁё' .-]*[A-Za-zА-Яа-яЁё]?$";
        private const string BirthdayPattern = @"^(0[1-9]|[12][0-9]|3[01])[-./](0[1-9]|1[0-2])[-./](19|20)\d{2}$";
        private const string PhonePattern = @"^\+7\(\d{3}\)\s\d{3}-\d{4}$";
        private const string AddressPattern = @"^[A-Za-zА-Яа-яЁё\s.,'-]+(?:\s+[0-9A-Za-zА-Яа-яЁё/\\-]+)*$";

        private static readonly DateTime MinBirthDate = new(1900, 1, 1);
        private static DateTime MaxBirthDate => DateTime.Now.AddYears(-14);

        public static ValidationResult Validate(FieldKind kind, string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.Fail("Field cannot be empty.");

            return kind switch
            {
                FieldKind.Name or FieldKind.Patronymic =>
                    Match(input, NamePattern, "latin, cyrillic characters and signs -'"),

                FieldKind.Surname =>
                    Match(input, SurnamePattern, "latin, cyrillic characters and signs -.'"),

                FieldKind.PhoneNumber =>
                    Match(input, PhonePattern, "Format: +7(XXX) XXX-XXXX"),

                FieldKind.Address =>
                    Match(input, AddressPattern, "Examples: 'New York, 5th Ave', 'Moscow, Arbat 25'"),

                FieldKind.Birthday =>
                    ValidateBirthday(input),

                _ => ValidationResult.Fail($"No validation rule for {kind}.")
            };
        }

        private static ValidationResult Match(string input, string pattern, string hint)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase)
                ? ValidationResult.Ok()
                : ValidationResult.Fail($"Invalid value. Correct format: {hint}");
        }

        private static ValidationResult ValidateBirthday(string input)
        {
            var hint = $"from 01.01.1900 to {MaxBirthDate:dd.MM.yyyy}";

            if (!Regex.IsMatch(input, BirthdayPattern))
                return ValidationResult.Fail($"Birth date is not valid. {hint}");

            var normalized = input.Replace('/', '.').Replace('-', '.');
            if (!DateTime.TryParseExact(normalized, "dd.MM.yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return ValidationResult.Fail($"Birth date is not valid. {hint}");

            return date >= MinBirthDate && date <= MaxBirthDate
                ? ValidationResult.Ok()
                : ValidationResult.Fail($"Birth date is not valid. {hint}");
        }
    }
}