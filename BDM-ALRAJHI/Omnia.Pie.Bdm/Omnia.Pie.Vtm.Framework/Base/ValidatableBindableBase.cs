namespace Omnia.Pie.Vtm.Framework.Base
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Credits : https://searchcode.com/codesearch/view/2306249/
	/// </summary>
	public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
	{
		private ErrorsContainer<ValidationResult> errorsContainer;
		protected ErrorsContainer<ValidationResult> ErrorsContainer
		{
			get
			{
				if (errorsContainer == null)
				{
					errorsContainer =
						new ErrorsContainer<ValidationResult>(pn => RaiseErrorsChanged(pn));
				}

				return errorsContainer;
			}
		}

		public ValidatableBindableBase()
		{

		}

		protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
		{
			ErrorsChanged?.Invoke(this, e);
		}

		public bool HasErrors
		{
			get { return ErrorsContainer.HasErrors; }
		}

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		[SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Method supports event.")]
		protected void RaiseErrorsChanged(string propertyName)
		{
			OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
		}

		public IEnumerable GetErrors(string propertyName)
		{
			return this.errorsContainer.GetErrors(propertyName);
		}

		private bool IsDynamicValidationEnabled { get; set; }

		protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			var result = base.SetProperty<T>(ref storage, value, propertyName);

			if (result && IsDynamicValidationEnabled)
			{
				ValidateInternal();
			}

			return result;
		}

		public bool Validate()
		{
			var result = ValidateInternal();
			IsDynamicValidationEnabled = !result;
			return result;
		}

		private bool ValidateInternal()
		{
			ErrorsContainer.ClearErrors();
			Validate(this);
			return !HasErrors;
		}

		private void Validate(ValidatableBindableBase validatable)
		{
			validatable.GetType()
						.GetProperties()
						.Where(p => p.GetCustomAttributes(typeof(ValidationAttribute), false).Any())
						.Where(p => p.CanRead)
						.ToList()
						.ForEach(p => Validate(validatable, p));
		}

		private void Validate(ValidatableBindableBase validatable, PropertyInfo property)
		{
			var value = property.GetValue(validatable);
			Validate(validatable, property.Name, value);
		}

		private void Validate(ValidatableBindableBase validatable, string propertyName, object value)
		{
			var validationContext = new ValidationContext(validatable) { MemberName = propertyName };
			var validationResults = new List<ValidationResult>();
			Validator.TryValidateProperty(value, validationContext, validationResults);
			validatable.ErrorsContainer.SetErrors(propertyName, validationResults);
		}

		protected void ValidateProperty(string propertyName, object value)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}

			ValidateProperty(new ValidationContext(this, null, null) { MemberName = propertyName }, value);
		}

		protected virtual void ValidateProperty(ValidationContext validationContext, object value)
		{
			if (validationContext == null)
			{
				throw new ArgumentNullException("validationContext");
			}

			var validationResults = new List<ValidationResult>();
			Validator.TryValidateProperty(value, validationContext, validationResults);

			ErrorsContainer.SetErrors(validationContext.MemberName, validationResults);
		}

	}
}