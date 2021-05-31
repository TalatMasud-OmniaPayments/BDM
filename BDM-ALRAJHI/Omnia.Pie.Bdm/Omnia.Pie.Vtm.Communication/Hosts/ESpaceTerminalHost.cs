namespace Omnia.Pie.Vtm.Communication
{
	using System.Windows.Forms;

	public partial class ESpaceTerminalHost : Form
	{
		public AxeSpaceMediaLib.AxeSpaceMedia Media;
		public ESpaceTerminalHost()
		{
			InitializeComponent();
			Media = axeSpaceMedia1;
		}
	}
}