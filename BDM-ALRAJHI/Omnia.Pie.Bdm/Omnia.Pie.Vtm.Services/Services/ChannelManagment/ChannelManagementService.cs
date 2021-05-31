namespace Omnia.Pie.Vtm.Services
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using Omnia.Pie.Vtm.Services.ISO.Request.ChannelManagement;
	using Omnia.Pie.Vtm.Services.ISO.Response.ChannelManagement;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public class ChannelManagementService : ServiceBase, IChannelManagementService
	{
		public ChannelManagementService(IResolver container, IServiceEndpoint endpointsProvider) : base(container, endpointsProvider)
		{

		}

		#region SendDeviceStatus

		public async Task<bool> SendDeviceStatus(List<Interface.Entities.ChannelManagement.DeviceStatus> devStatus, bool cashReplenishment = false, bool coinReplenishment = false)
		{
			try
			{
				return await ExecuteFaultHandledOperationAsync<DeviceStatusRequest, bool>(async c =>
				{
					var response = await SendDeviceStatusAsync(GetSendDeviceStatusRequest(devStatus, cashReplenishment, coinReplenishment));
					return ToChannelManagementService(response);
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				return false;
			}
		}

		private async Task<DeviceStatusResponse> SendDeviceStatusAsync(DeviceStatusRequest request)
			=> await ExecuteServiceAsync<DeviceStatusRequest, DeviceStatusResponse>(request);

		private DeviceStatusRequest GetSendDeviceStatusRequest(List<Interface.Entities.ChannelManagement.DeviceStatus> devStatus, bool cashReplenishment, bool coinReplenishment)
		{
			var req = new DeviceStatusRequest
			{
				DeviceStatus = new List<DeviceStatus>(),
				CashReplenishment = cashReplenishment, 
				CoinReplenishment = coinReplenishment,
			};

			if (devStatus != null)
			{
				foreach (var item in devStatus)
				{
					req.DeviceStatus.Add(new DeviceStatus()
					{
						DeviceName = item?.DeviceName,
						ErrorCode = item?.ErrorCode,
						Status = item?.Status,
						OperationalStatus = GetOparationStatus(item)
					});
				}
			}

			return req;
		}

		private OperationalStatus GetOparationStatus(Interface.Entities.ChannelManagement.DeviceStatus item)
		{
			var status = new OperationalStatus
			{
				InkStatus = item?.OperationalStatus?.InkStatus,
                PaperStatus = item?.OperationalStatus?.PaperStatus
            };

			if (item != null && item?.OperationalStatus != null && item?.OperationalStatus?.Cassettes != null)
			{
				status.Cassettes = new List<Cassette>();

				foreach (var itm in item.OperationalStatus.Cassettes)
				{
					status.Cassettes.Add(new Cassette()
					{
						Name = itm.Name,
						Type = itm.Type,
						Count = itm.Count,
					});
				}
			}

			return status;
		}

		private bool ToChannelManagementService(DeviceStatusResponse response) => response.Result;

		#endregion

		#region "Insert Events"

		public async Task<InsertEventResult> InsertEventAsync(string Event, string value)
		{
			try
			{
				if (string.IsNullOrEmpty(Event)) throw new ArgumentNullException(nameof(Event));
				if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

				return await ExecuteFaultHandledOperationAsync<InsertEventRequest, InsertEventResult>(async c =>
				{
					var response = await InsertEventAsync(ToCustomerIdentifierRequest(Event, value));
					return ToInsertEvent(response);
				});
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
				return null;
			}
		}

		private async Task<InsertEventResponse> InsertEventAsync(InsertEventRequest request)
			=> await ExecuteServiceAsync<InsertEventRequest, InsertEventResponse>(request);

		private InsertEventRequest ToCustomerIdentifierRequest(string Event, string value) => new InsertEventRequest
		{
			Event = Event,
			Value = value
		};

		private InsertEventResult ToInsertEvent(InsertEventResponse response) => new InsertEventResult
		{
			Result = response?.InsertEvent?.Result
		};

		#endregion

	}
}