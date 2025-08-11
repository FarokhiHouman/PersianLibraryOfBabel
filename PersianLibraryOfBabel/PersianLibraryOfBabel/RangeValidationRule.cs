// Simple, SOLID-compliant validation rule for numeric ranges
using System.Globalization;
using System.Windows.Controls;


namespace PersianLibraryOfBabel;
public class RangeValidationRule : ValidationRule {
	public int Minimum { get; set; }
	public int Maximum { get; set; }

	public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
		if (!int.TryParse(value?.ToString(), out int number))
			return new ValidationResult(false, "لطفاً عدد معتبر وارد کنید.");
		if (number < Minimum ||
			number > Maximum)
			return new ValidationResult(false, $"عدد باید بین {Minimum} و {Maximum} باشد.");
		return ValidationResult.ValidResult;
	}
}