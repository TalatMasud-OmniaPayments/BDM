namespace Omnia.Pie.Vtm.Framework.Reports
{
    using Microsoft.Reporting.WinForms;
    using Omnia.Pie.Vtm.Framework.Base;
    using Omnia.Pie.Vtm.Framework.ControlExtenders;
    using Omnia.Pie.Vtm.Framework.Interface.Reports;
    using Omnia.Pie.Vtm.Framework.Reports.Template;
    using System.Collections.Generic;

    internal class NewAccountReport : BaseReport<NewAccountReportData>
    {
        protected override void InitializeReport(LocalReport report, NewAccountReportData reportData)
        {
            base.InitializeReport(report, reportData);

            var accountDataSource = new ReportDataSource
            {
                Name = nameof(NewAccountData) + "Set",
                Value = new List<NewAccountData>
                {
                    new NewAccountData
                    {
                        WelcomeMessage = reportData.WelcomeMessage,
                        CustomerId = reportData.CustomerId,
                        CustomerName = reportData.CustomerName,
                        AccountNumber = reportData.AccountNumber,
                        AccountIBAN = reportData.AccountIBAN,
                        AccountType = reportData.AccountType,
                        AccountCurrency = reportData.AccountCurrency,
                        IsChequeBook = reportData.IsChequeBook,
                        Signature1Base64Content = reportData.Signature1?.ToBase64String(),
                        Signature2Base64Content = reportData.Signature2?.ToBase64String(),
                        CustomerEidNo = reportData.CustomerEidNo,
                        CustomerMobile = reportData.CustomerMobile,
                        CustomerEmail = reportData.CustomerEmail,
                        CustomerPassport = reportData.CustomerPassport,
                        CheckedById = reportData.CheckedById,
                        CheckedByName = reportData.CheckedByName
                    }
                }
            };

            report.DataSources.Add(accountDataSource);

            /*var statementItemsDataSource = new ReportDataSource
            {
                Name = nameof(StatementItem) + "DataSet",
                Value = reportData.Items ?? new List<StatementItem>()
            };

            report.DataSources.Add(statementItemsDataSource);*/
        }
    }
}
