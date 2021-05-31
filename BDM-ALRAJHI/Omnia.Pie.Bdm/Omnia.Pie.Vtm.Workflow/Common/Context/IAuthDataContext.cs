namespace Omnia.Pie.Vtm.Workflow.Common.Context
{
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
    using Omnia.Pie.Vtm.Services.Interface.Entities;
    using Omnia.Pie.Vtm.ServicesNdc.Interface.Entities;
	using System.Collections.Generic;

	public interface IAuthDataContext
	{
		Card Card { get; set; }
		string EIdNumber { get; set; }
		NdcCard SelectedCard { get; set; }
		List<NdcCard> Cards { get; set; }
		string CustomerId { get; set; }
		string Otp { get; set; }
		bool OtpMatched { get; set; }
		string Uuid { get; set; }
		string Cif { get; set; }
		string Pin { get; set; }
		bool Authenticated { get; set; }
		bool CifMod { get; set; }
		bool SelfServiceMode { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        /*string Usertype { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string Mobile { get; set; }*/
        string Fingerprint { get; set; }
        List<UserTypes> allUserTypes { get; set; }
        List<Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo> allUsers { get; set; }
        Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo newUserInfo { get; set; }
        Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo loggedInUserInfo { get; set; }
        bool AuthenticatedByFingerprint { get; set; }
        bool isOnlineTran { get; set; }
        bool isNetworkAvailable { get; set; }
        bool isTransactionStarted { get; set; }
    }
}