using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Supervisor.Shell.Constants
{
	public class ConfigurationKeys
	{
		public static readonly string ActiveDirectoryClients	= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.ActiveDirectoryClient']";
		public static readonly string TransactionClient			= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.TransactionClient']";
		public static readonly string AuthenticationClient		= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.AuthenticationClient']";
		public static readonly string AtmClient					= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.AtmClient']";
		public static readonly string CommunicationClient		= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.CommunicationClient']";
		public static readonly string CustomerClient			= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.CustomerClient']";
		public static readonly string CustomerV2Client			= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.CustomerV2Client']";
		public static readonly string ArrangementClient			= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.ArrangementClient']";
		public static readonly string CommonClient				= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.CommonClient']";
		public static readonly string ResourceRequestClient		= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.ResourceRequestClient']";
		public static readonly string InvolvedPartyClient		= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.InvolvedPartyClient']";
		public static readonly string ResourceClient			= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.ResourceClient']";
		public static readonly string DmsClient					= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.DmsClient']";
		public static readonly string AlertClient				= $"//clients//client[@class='Omnia.Pie.Client.Repositories.Clients.AlertClient']";

		public static List<string> GetAllKeys()
		{
			return new List<string>()
			{
				ActiveDirectoryClients
				,TransactionClient
				,AuthenticationClient
				,AtmClient
				,CommunicationClient
				,CustomerClient
				,CustomerV2Client
				,ArrangementClient
				,CommonClient
				,ResourceRequestClient
				,InvolvedPartyClient
				,ResourceClient
				,DmsClient
				,AlertClient
			};
		}
	}
}
