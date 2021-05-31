namespace Omnia.Pie.Vtm.Devices.Interface
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using System;
	using System.Threading.Tasks;

	public interface IPinPad : IDevice
	{
		event EventHandler<PinPadDigitPressedEventArgs> DigitPressed;
        event EventHandler<string> PinPadStatusChanged;
        event EventHandler EnterPressed;
		event EventHandler CancelPressed;
		event EventHandler ClearPressed;

		void StartReading();
		Task StartPinReading();
		Task StopPinReading();

		/// <summary>
		/// Builds encrypted representation of the PIN code on the pin pad device.
		/// </summary>
		/// <param name="card">Card for which PIN block should be built.</param>
		/// <returns>HEX string that contains the encrypted PIN code.</returns>
		Task<string> BuildPinBlockAsync(Card card);

		/// <summary>
		/// Build encrypted Pin Using Die bold
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		Task<string> BuildPinBlockDieboldAsync(Card card);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="card"></param>
		/// <returns></returns>
		Task<string> BuildPinBlockDieboldAsync(string card);

		/// <summary>
		/// Builds encrypted representation of the PIN code on the pin pad device.
		/// Used for CIF & EIDA
		/// </summary>
		/// <param name="formattedPan"></param>
		/// <returns></returns>
		Task<string> BuildPinBlockAsync(string formattedPan);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pLoadKeyName"></param>
		/// <param name="pEncKeyName"></param>
		/// <param name="pKeyValue"></param>
		/// <param name="pUse"></param>
		/// <param name="pStrKeyCheckValue"></param>
		void ImportKey(string pLoadKeyName, string pEncKeyName, string pKeyValue, string pUse, out string pStrKeyCheckValue);
	}

	public class PinPadDigitPressedEventArgs : EventArgs
	{
		public PinPadDigitPressedEventArgs(int digit)
		{
			if (digit < 0 || digit > 9)
			{
				throw new ArgumentOutOfRangeException(nameof(digit), "Digit must be in the range from 0 to 9.");
			}
			Digit = digit;
		}

		public int Digit { get; }
	}
}