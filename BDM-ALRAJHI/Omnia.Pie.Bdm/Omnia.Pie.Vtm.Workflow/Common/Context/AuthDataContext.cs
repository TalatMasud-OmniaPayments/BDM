namespace Omnia.Pie.Vtm.Workflow.Common.Context
{
	using System.Collections.Generic;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.ServicesNdc.Interface.Entities;
    using Omnia.Pie.Vtm.Services.Interface.Entities;

    public class AuthDataContext : BaseContext, IAuthDataContext
	{
		public Card Card { get; set; }
		public string EIdNumber { get; set; }
		public bool Authenticated { get; set; }
		public string Otp { get; set; }
		public bool OtpMatched { get; set; }
		public string Uuid { get; set; }
		public string Pin { get; set; }
		public string Cif { get; set; }
		public NdcCard SelectedCard { get; set; }
		public List<NdcCard> Cards { get; set; }
        public List<Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo> allUsers { get; set; }
        public bool CifMod { get; set; }
		public bool SelfServiceMode { get; set; }
        public string Username { get; set; }
         public string Password { get; set; }
        /*public string Usertype { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        */
        public string Fingerprint { get; set; }
        public List<UserTypes> allUserTypes { get; set; }
        public Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo newUserInfo { get; set; }
        public Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo loggedInUserInfo { get; set; }

        public bool AuthenticatedByFingerprint { get; set; }
        public bool isOnlineTran { get; set; }
        public bool isNetworkAvailable { get; set; }
        public bool isTransactionStarted { get; set; }
    }
}