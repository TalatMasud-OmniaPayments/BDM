namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.IO;
	using System.Threading.Tasks;

	public interface IStatementPrinter : IPrinter
	{
		Task PrintAsync(FileInfo image, PaperSource paperSource);
		string GetInkStatu();
		string GetTonerStatus();
		void TurnOnGuideLights();
		void TurnOffGuideLights();
		string GetChequePaperStatus();
	}
}