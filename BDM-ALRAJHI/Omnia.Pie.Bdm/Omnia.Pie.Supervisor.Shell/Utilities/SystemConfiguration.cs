using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Framework.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Omnia.Pie.Supervisor.Shell.Utilities
{
	public class SystemConfiguration
	{
		[DllImport("kernel32.dll")]
		private static extern void GetSystemTime(ref SystemTime lpSystemTime);

		[DllImport("kernel32.dll")]
		private static extern uint SetSystemTime(ref SystemTime lpSystemTime);

		[DllImport("kernel32.dll")]
		static extern uint GetLastError();

		private readonly ILogger _logger = ServiceLocator.Instance.Resolve<ILogger>();

		private struct SystemTime
		{
			public ushort wYear;
			public ushort wMonth;
			public ushort wDayOfWeek;
			public ushort wDay;
			public ushort wHour;
			public ushort wMinute;
			public ushort wSecond;
			public ushort wMilliseconds;
		}

		private void GetTime()
		{
			SystemTime stime = new SystemTime();
			GetSystemTime(ref stime);
		}
		public void SetTime(ushort date, ushort month, ushort year, ushort hour, ushort min, ushort sec)
		{
			SystemTime systime = new SystemTime();

			GetSystemTime(ref systime);

			systime.wYear = year;
			systime.wMonth = month;
			systime.wDay = date;
			systime.wHour = hour;
			systime.wMinute = min;
			systime.wSecond = sec;

			var a = SetSystemTime(ref systime);
		}

		public void SetSystemTimeZone(string timeZoneId)
		{
			try
			{
				if (TimeZone.CurrentTimeZone.StandardName != timeZoneId)
				{
					var process = Process.Start(new ProcessStartInfo
					{
						FileName = "tzutil.exe",
						Arguments = "/s \"" + timeZoneId + "\"",
						UseShellExecute = false,
						CreateNoWindow = true
					});

					if (process != null)
					{
						process.WaitForExit();
						TimeZoneInfo.ClearCachedData();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Timezone {timeZoneId} cannot be set");
				_logger.Exception(ex);
			}
		}

		public void SetIP(string IPAddress)
		{
			try
			{
				string subnetMask = GetSubnetMask(IPAddress);
				ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
				ManagementObjectCollection objMOC = objMC.GetInstances();

				foreach (ManagementObject objMO in objMOC)
				{
					if ((bool)objMO["IPEnabled"])
					{
						ManagementBaseObject setIP;
						ManagementBaseObject newIP =
							objMO.GetMethodParameters("EnableStatic");

						newIP["IPAddress"] = new string[] { IPAddress };
						newIP["SubnetMask"] = new string[] { subnetMask };

						setIP = objMO.InvokeMethod("EnableStatic", newIP, null);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Error while setting local IP Address to {IPAddress}");
				_logger.Exception(ex);
			}
		}

		private string GetSubnetMask(string ipaddress)
		{
			uint firstOctet = ReturnFirstOctet(ipaddress);
			if (firstOctet >= 0 && firstOctet <= 127)
				return "255.0.0.0";
			else if (firstOctet >= 128 && firstOctet <= 191)
				return "255.255.0.0";
			else if (firstOctet >= 192 && firstOctet <= 223)
				return "255.255.255.0";
			else return "0.0.0.0";
		}

		private uint ReturnFirstOctet(string IpAddress)
		{
			IPAddress iPAddress = IPAddress.Parse(IpAddress);
			byte[] byteIP = iPAddress.GetAddressBytes();
			uint ipInUint = (uint)byteIP[0];
			return ipInUint;
		}

		public string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new InvalidOperationException("Local IP Address Not Found!");
		}

		public bool PingHost(string nameOrAddress)
		{
			bool pingable = false;
			Ping pinger = new Ping();
			try
			{
				PingReply reply = pinger.Send(nameOrAddress);
				pingable = reply.Status == IPStatus.Success;
			}
			catch (PingException ex)
			{
				_logger.Error($"Address {nameOrAddress} is not pingable");
				_logger.Exception(ex);

				return pingable;
			}
			return pingable;
		}
	}
}
