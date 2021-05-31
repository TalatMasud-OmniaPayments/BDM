namespace Omnia.Pie.Vtm.Framework.Interface.Engine
{
	using Omnia.Pie.Vtm.Framework.Interface.Converters;
	using Omnia.Pie.Vtm.Framework.Interface.Configuration;
	using System;

	public class ReceiptModel<TReceipt>
	{
		public ReceiptModel(TReceipt receipt, ReceiptMetadata metadata, bool isRightToLeft, bool isMarkupEnabled)
		{
			if (receipt == null) throw new ArgumentNullException(nameof(receipt));
			Receipt = receipt;
			Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

			Output = new ReceiptOutput(ReceiptsConfiguration.OutputWidth, isRightToLeft, isMarkupEnabled);
		}

		public ReceiptMetadata Metadata { get; }
		public TReceipt Receipt { get; }
		public ReceiptOutput Output { get; }

		#region Converters

		private readonly Lazy<MaskedCardNumberConverter> maskedCardNumber = new Lazy<MaskedCardNumberConverter>(() => new MaskedCardNumberConverter());
		public MaskedCardNumberConverter MaskedCardNumber => maskedCardNumber.Value;

		#endregion Converters

		#region Binding

		public string Binding(object value, string targetNullValue)
		{
			var result = value?.ToString();

			if (string.IsNullOrEmpty(result))
			{
				result = targetNullValue;
			}

			return result;
		}

		public string Binding(object value, IValueConverter converter, object parameter, string targetNullValue)
		{
			var result = converter.Convert(value, parameter);
			return Binding(result, targetNullValue);
		}

		public string Binding(object value, IValueConverter converter, string targetNullValue) => Binding(value, converter, null, targetNullValue);

		public string Binding(int value, string format) => value.ToString(format);

		public string Binding(int? value, string format, string targetNullValue) => value == null ? targetNullValue : Binding(value.Value, format);

		public string Binding(double value, string format) => value.ToString(format);

		public string Binding(double? value, string format, string targetNullValue) => value == null ? targetNullValue : Binding(value.Value, format);

		public string Binding(DateTime value, string format) => value.ToString(format);

		public string Binding(DateTime? value, string format, string targetNullValue) => value == null ? targetNullValue : Binding(value.Value, format);

		#endregion Binding
	}
}