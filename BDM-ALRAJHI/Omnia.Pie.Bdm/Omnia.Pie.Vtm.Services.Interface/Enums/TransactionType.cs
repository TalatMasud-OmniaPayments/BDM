using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services.Interface
{
	public enum TransactionType
	{
		//SSCashWithdrawalDebitCardCif = 2,
		//SSCashWithdrawalDebitCardEida = 3,
		//RTCashWithdrawalDebitCardCif = 4,
		//RTCashWithdrawalDebitCardEida = 5,
		//SSCashWithdrawalCreditCardCif = 7,
		//SSCashWithdrawalCreditCardEida = 8,
		//SSCashDepositCreditCardCif = 10,
		//SSCashDepositCreditCardEida = 11,
		//SSCashDepositAccountCif = 13,
		//SSCashDepositAccountEida = 14,
		//RTChequeDeposit = 25,
		SSCashWithdrawalDebitCard = 1,
		SSCashWithdrawalCreditCard = 6,
		SSCashDepositCreditCard = 9,
		SSCashDepositAccountDebitCard = 12,
		SSCashDepositAccountCardless = 15,
		SSChequeDepositCardless = 16,
		SSChequeDepositDebitCard = 17,
		SSStatementPrint = 18,
		RTStatementPrint = 19,
		SSIbanPrint = 20,
		RTIbanPrint = 21,
		SSProductsInformation = 22,
		RTCashWithdrawal = 23,
		RTChequeEncashment = 24,
		SSBalanceInquiry = 26,
		RTBalanceInquiry = 27,
		RTCashDepositAccount = 28,
		RTCashDepositCreditCard = 30,
		RTChequePrint = 31,
		SSLCRequest = 32,
		RTLCRequest = 33,
		SSNLCRequest = 34,
		RTNLCRequest = 35,
	}
}