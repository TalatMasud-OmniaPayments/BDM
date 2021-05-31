namespace Omnia.Pie.Vtm.Framework.Base
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public static class CustomValidation
	{
		public static ValidationResult Validate<T>(ValidationContext context, Func<T, bool> validate, string errorMessage = null) where T : ValidatableBindableBase
		{
			var result = ValidationResult.Success;

			var viewModel = context.ObjectInstance as T;
			if (viewModel != null && validate(viewModel) == false)
			{
				result = new ValidationResult(errorMessage ?? string.Empty);
			}

			return result;
		}

		public class MultiplesOfHundredAttribute : ValidationAttribute
		{
			public override bool IsValid(object amount)
			{
				double am = (double)amount;
				var a = am % 100;
				if (a == 0)
					return true;
				else
					return false;
			}
		}
	}
}