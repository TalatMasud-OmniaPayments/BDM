namespace Omnia.Pie.Vtm.Workflow.MainMenu.Steps
{
    using Omnia.Pie.Supervisor.Shell.Service;
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.DataAccess.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
    using System;
    //using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using Microsoft.Practices.Unity;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Workflow.Common;
    using Omnia.Pie.Vtm.DataAccess.Interface.Entities;
    using System.Collections.Generic;
    using System.Windows.Threading;
    using System.Configuration;
    using System.Globalization;
    using static Omnia.Pie.Vtm.Services.ServiceBase;
    using System.Linq;

    public class LanguageSelectionStep : WorkflowStep
	{
        private readonly ITransactionStore _transactionStore = ServiceLocator.Instance.Resolve<ITransactionStore>();
        private readonly IUsersStore _userStore = ServiceLocator.Instance.Resolve<IUsersStore>();
        private TaskCompletionSource<bool> _completion;
        private static Timer transactionTimer;
        static Timer downloadUsersTimer;
        //bool shouldDownloadUsers = false;

        public LanguageSelectionStep(IResolver container) : base(container)
		{

		}
        public static string ToString(DateTime? v, DateFormat format = DateFormat.Default) => v?.ToString(ToDateTimeFormatString(format));
        private static string ToDateTimeFormatString(DateFormat df)
        {
            string result;

            switch (df)
            {
                case DateFormat.ddMMyyyy:
                    result = "dd/MM/yyyy";
                    break;
                case DateFormat.yyyyMMdd:
                    result = "yyyyMMdd";
                    break;
                case DateFormat.MMddyyyy:
                    result = "MM/dd/yyyy";
                    break;
                case DateFormat.yyyyMMddHHmm:
                    result = "yyyyMMddHHmm";
                    break;
                case DateFormat.yyyyMMddDashed:
                    result = "yyyy-MM-dd";
                    break;
                case DateFormat.yyyyMMddTHHmmsssssZ:
                    result = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }
        public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Language Selection");
            
            _completion = new TaskCompletionSource<bool>();
            StartSynchOfflineTimer();

            if (SynchNewUsers()) { 
                GetNewUsers();
                StopnDownloadUserTimer();
                Download_Timer();
            }
            /*var date = "2020-10-27 14:00:55";
            //var date1 = "2020-08-04T10:28:30:111Z";
            DateTime result;

            if (DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                //result;
            }
            if (DateTime.TryParseExact(date1, "yyyy-MM-dd'T'HH:mm:ss:fff'Z'", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                //result;
            }
            var temp = result.ToString();
            var temp1 = ToString(result, DateFormat.yyyyMMddTHHmmss);*/

            _navigator.RequestNavigationTo<ILanguageSelectionViewModel>((viewModel) =>
			{
				viewModel.StartAction = () =>
				{
                    StopTimer();
					PlayWelcomeSound();
					_completion.TrySetResult(true);
				};
			});

			return await _completion.Task;
		}


        public bool SynchNewUsers()
        {
            bool shouldDownloadUsers = false;
            var shouldSynchNewUsers = ConfigurationManager.AppSettings["ShouldSynchNewUsers"].ToString();
            bool.TryParse(shouldSynchNewUsers, out shouldDownloadUsers);

            return shouldDownloadUsers;

        }
        public void StartSynchOfflineTimer()
        {

           
            var time = ConfigurationManager.AppSettings["SynchOfflineData"].ToString();
            int synchTime;

            if (!Int32.TryParse(time, out synchTime))
            {
                synchTime = 10;
            }

            transactionTimer = new Timer();
            transactionTimer.Interval = synchTime *1000;

            transactionTimer.Elapsed += OnTimedEvent;
            transactionTimer.AutoReset = true;
            transactionTimer.Enabled = true;
        }
        public void StopTimer()
        {
            if (transactionTimer != null) {
                transactionTimer.Enabled = false;
                transactionTimer = null;
            }
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (_networkStatus.isConnected())
            {
                //StopTimer();
                
                //LoadWaitScreen();
                var _transactionService = _container.Resolve<ITransactionService>();
                var offlineTrasnactions = _transactionStore.GetAllOfflineTransaction();
                if (offlineTrasnactions.Count > 0) { 
                    
                
                    UploadAllOfflineTransactions(offlineTrasnactions, _transactionService);
                }
                
            }
            else
            {
                Console.WriteLine("network not available.");
            }

            //Console.WriteLine("Raised: {0}", e.SignalTime);
        }

        private async void GetNewUsers()
        {
            try { 
            var userId = _userStore.getLastUserId();
                await DownloadNewUsersAsync(userId);
                //await DownloadNewUsersAsync(3);         // hardcoded
                Console.WriteLine("DownloadNewUsers");
            }
            catch (Exception ex)
            {
                _logger?.Info($"No new users available to download: " + ex.Message);
                //StopTimer();
                //Console.WriteLine("No new users to download");
            }

            //return userInfo;
        }
        public async Task DownloadNewUsersAsync(int userId1)
        {
            //var users = new List<UserInfo>();
            var _userInfoService = _container.Resolve<IUserService>();
            var _users = await _userInfoService.GetNewUsersAsync("", userId1.ToString());

            _userStore.SaveNewUser(_users);
            Console.WriteLine("DownloadNewUsers");
            //return _users;

        }

        public async void UploadAllOfflineTransactions(List<DeviceTransaction> offlineTrasnactions, ITransactionService _transactionService)
        {

            _logger?.Info($"Sync Start: Offline Transactions");
            


            foreach (var transaction in offlineTrasnactions)
            {
                var cashDeposited = _transactionStore.GetOfflineCashDepositedOf(transaction.id);
                var depositedDenominations = _transactionStore.GetOfflineDepositedDenomsOf(transaction.id);

                var _cashDepositToAccount = await _transactionService.CashDepositAsync(
                        transaction.Username,
                        depositedDenominations,
                        cashDeposited);

                if (_cashDepositToAccount.ResponseCode == "000")
                {
                    bool isUpdated = await _transactionStore.UpdateOfflineTransaction(transaction.id, _cashDepositToAccount.ResponseCode);
                    _logger?.Info($"Transaction Synced: OfflineId: {transaction.id} And Service Response: {_cashDepositToAccount.ResponseCode}");
                    _logger?.Info($"Transaction Synced: isUpdatedLocally: {isUpdated}");
                    //Console.WriteLine("Upload offline transactions on the server");
                }

            }

            _logger?.Info($"Sync Finished: Offline Transactions");
        }

        public bool isValidTime(string downloadTime)
        {
            DateTime dt;
            //DateTime dateTime = DateTime.ParseExact(downloadTime, "HH:mm", CultureInfo.InvariantCulture);

            if(DateTime.TryParseExact(downloadTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Download_Timer()
        {

            

            string downloadTime = ConfigurationManager.AppSettings["DownloadNewUsers"].ToString();
            int configMinute = 0 ;
            int configHour = 0;

            if (String.IsNullOrEmpty(downloadTime) || !isValidTime(downloadTime))
            {
                _logger?.Info($"Download users time {downloadTime} is invalid, re-adjusting downloadNewuUsersTime to 00:00");
                configMinute = 0;
                configHour = 0;
            }
            else
            {
                string[] timeSplit = downloadTime.Split(':');

                if (!Int32.TryParse(timeSplit[0], out configHour))
                {
                    configHour = 0;
                }


                if(timeSplit.ElementAtOrDefault(1) != null)
                {
                    if (!Int32.TryParse(timeSplit[1], out configMinute))
                    {
                        configMinute = 0;
                    }
                }

                
                /*if (configHour > 23 || configMinute > 59 || configHour < 0 || configMinute < 0)
                {
                    _logger?.Info($"Download users time {configHour} :{configMinute} is invalid, re-adjusting it to 00:00");
                    configMinute = 0;
                    configHour = 0;
                }*/
            }
            //configMinute = 4;

            DateTime nowTime = DateTime.Now;
            DateTime synchTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, configHour, configMinute, 0, 0);
            if (nowTime > synchTime)
                synchTime = synchTime.AddDays(1);

            double tickTime = (synchTime - nowTime).TotalMilliseconds;
            //tickTime = Math.Abs(tickTime);
            downloadUsersTimer = new Timer(tickTime);
            downloadUsersTimer.Elapsed += downloadUsers_Elapsed;
            downloadUsersTimer.Start();
        }

        private void downloadUsers_Elapsed(object sender, ElapsedEventArgs e)
        {

            StopnDownloadUserTimer();
            GetNewUsers();
            Download_Timer();
        }

        public void StopnDownloadUserTimer()
        {
            if (downloadUsersTimer != null)
            {
                downloadUsersTimer.Stop();
                downloadUsersTimer = null;
            }
        }
        public override void Dispose()
		{

		}
	}
}