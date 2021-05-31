using Omnia.Pie.Vtm.Services.Interface.Entities;

namespace Omnia.Pie.Vtm.Services.ISO.Request.Authentication
{
	public class AccountsRequest : RequestBase
	{
		public string CustomerIdentifier { get; set; }
        public string Username { get; set; }
        public AccountCriterion ConditionId { get; set; }
	}
}