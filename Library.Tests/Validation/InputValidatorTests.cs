using Library.Presentation.Validation;

namespace Library.Tests.Validation;

public class InputValidatorTests
{
	// The field kind is passed as a string: FieldKind is internal, and xUnit test methods must be public
	[Theory]
	[InlineData("Name", "Ivan")]
	[InlineData("Name", "Анна-Мария")]
	[InlineData("Surname", "O'Brien")]
	[InlineData("PhoneNumber", "+7(777) 123-4567")]
	[InlineData("Address", "Moscow, Arbat 25")]
	[InlineData("Birthday", "15.05.1990")]
	[InlineData("Birthday", "15/05/1990")]
	public void Validate_CorrectInput_IsValid(string field, string input)
	{
		var result = InputValidator.Validate(Enum.Parse<FieldKind>(field), input);

		Assert.True(result.IsValid, result.Error);
	}

	[Theory]
	[InlineData("Name", "Ivan1")]
	[InlineData("PhoneNumber", "87771234567")]
	[InlineData("Birthday", "31.02.1990")]  // no such date
	[InlineData("Birthday", "01.01.1899")]  // before 1900
	[InlineData("Name", "   ")]
	public void Validate_WrongInput_IsInvalid(string field, string input)
	{
		var result = InputValidator.Validate(Enum.Parse<FieldKind>(field), input);

		Assert.False(result.IsValid);
		Assert.NotNull(result.Error);
	}

	[Fact]
	public void Validate_BirthdayYoungerThan14_IsInvalid()
	{
		var tooYoung = DateTime.Today.AddYears(-10).ToString("dd.MM.yyyy");

		var result = InputValidator.Validate(FieldKind.Birthday, tooYoung);

		Assert.False(result.IsValid);
	}
}
