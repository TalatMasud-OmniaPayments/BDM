namespace Omnia.Pie.Vtm.Framework.Exceptions
{
	using Omnia.Pie.Vtm.Framework.Properties;
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class AuthorizationValidationException : ApplicationException
	{
		public AuthorizationValidationException(string msg) : base(msg)
		{
		}

		public AuthorizationValidationException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}
	}


	[Serializable]
	public class ServiceException : Exception
	{
		public ServiceException() : base(Resources.RepositoryExceptionMessage)
		{
		}

		public ServiceException(string msg) : base(msg)
		{
		}

		public ServiceException(string msg, Exception innerException) : base(msg, innerException)
		{
		}

		protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

    [Serializable]
    public class FingerprintDeviceException : ServiceException
    {
        public FingerprintDeviceException()
            : base(Resources.BlockedCardExceptionMessage)
        {
        }

        public FingerprintDeviceException(string message)
            : base(message)
        {
        }

        public FingerprintDeviceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FingerprintDeviceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
    [Serializable]
	public class BlockedCardException : ServiceException
	{
		public BlockedCardException()
			: base(Resources.BlockedCardExceptionMessage)
		{
		}

		public BlockedCardException(string message)
			: base(message)
		{
		}

		public BlockedCardException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected BlockedCardException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class CardCaptureException : ServiceException
	{
		public CardCaptureException()
			: base(Resources.CardCaptureExceptionMessage)
		{
		}

		public CardCaptureException(string message)
			: base(message)
		{
		}

		public CardCaptureException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected CardCaptureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class ChequeBookPendingException : ServiceException
	{
		public ChequeBookPendingException()
			: base(Resources.ChequeBookPendingExceptionMessage)
		{
		}

		public ChequeBookPendingException(string message)
			: base(message)
		{
		}

		public ChequeBookPendingException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ChequeBookPendingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class DuplicateBeneficiaryException : ServiceException
	{
		public DuplicateBeneficiaryException()
			: base(Resources.DuplicateBeneficiaryExceptionMessage)
		{
		}

		public DuplicateBeneficiaryException(string message)
			: base(message)
		{
		}

		public DuplicateBeneficiaryException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected DuplicateBeneficiaryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class DuplicateLeadException : ServiceException
	{
		public DuplicateLeadException()
			: base(Resources.DuplicateLeadExceptionMessage)
		{
		}

		public DuplicateLeadException(string message)
			: base(message)
		{
		}

		public DuplicateLeadException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected DuplicateLeadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class ExceededOtpException : ServiceException
	{
		public ExceededOtpException()
			: base(Resources.ExceededOtpExceptionMessage)
		{
		}

		public ExceededOtpException(string message)
			: base(message)
		{
		}

		public ExceededOtpException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ExceededOtpException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class ExceededPinException : ServiceException
	{
		public ExceededPinException() : base(Resources.ExceededPinExceptionMessage)
		{

		}

		public ExceededPinException(string message) : base(message)
		{

		}

		public ExceededPinException(string message, Exception inner) : base(message, inner)
		{

		}

		protected ExceededPinException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}

	[Serializable]
	public class ExceededWithdrawalFrequencyException : ServiceException
	{
		public ExceededWithdrawalFrequencyException()
			: base(Resources.ExceededWithdrawalLimitExceptionMessage)
		{
		}

		public ExceededWithdrawalFrequencyException(string message)
			: base(message)
		{
		}

		public ExceededWithdrawalFrequencyException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ExceededWithdrawalFrequencyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class ExceededWithdrawalLimitException : ServiceException
	{
		public ExceededWithdrawalLimitException()
			: base(Resources.ExceededWithdrawalLimitExceptionMessage)
		{
		}

		public ExceededWithdrawalLimitException(string message)
			: base(message)
		{
		}

		public ExceededWithdrawalLimitException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ExceededWithdrawalLimitException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class ExceededPerTransactionLimitException : ServiceException
	{
		public ExceededPerTransactionLimitException()
			: base(Resources.ExceededWithdrawalLimitExceptionMessage)
		{
		}

		public ExceededPerTransactionLimitException(string message)
			: base(message)
		{
		}

		public ExceededPerTransactionLimitException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected ExceededPerTransactionLimitException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class CardNotInServiceException : ServiceException
	{
		public CardNotInServiceException() : base(Resources.ExpiredCardExceptionMessage)
		{
		}

		public CardNotInServiceException(string message) : base(message)
		{
		}

		public CardNotInServiceException(string message, Exception inner) : base(message, inner)
		{
		}

		protected CardNotInServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class ExpiredCardException : ServiceException
	{
		public ExpiredCardException() : base(Resources.ExpiredCardExceptionMessage)
		{
		}

		public ExpiredCardException(string message) : base(message)
		{
		}

		public ExpiredCardException(string message, Exception inner) : base(message, inner)
		{
		}

		protected ExpiredCardException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class InsufficientAccountBalanceException : ServiceException
	{
		public InsufficientAccountBalanceException()
			: base(Resources.InsufficientAccountBalanceExceptionMessage)
		{
		}

		public InsufficientAccountBalanceException(string message)
			: base(message)
		{
		}

		public InsufficientAccountBalanceException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InsufficientAccountBalanceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class InsufficientCreditCardBalanceException : ServiceException
	{
		public InsufficientCreditCardBalanceException()
			: base(Resources.InsufficientCreditCardBalanceExceptionMessage)
		{
		}

		public InsufficientCreditCardBalanceException(string message) : base(message)
		{
		}

		public InsufficientCreditCardBalanceException(string message, Exception inner) : base(message, inner)
		{
		}

		protected InsufficientCreditCardBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class InvalidAccountException : ServiceException
	{
		public InvalidAccountException()
			: base(Resources.InvalidAccountExceptionMessage)
		{
		}

		public InvalidAccountException(string message)
			: base(message)
		{
		}

		public InvalidAccountException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidAccountException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
    [Serializable]
    public class InvalidLoginException : ServiceException
    {
        public InvalidLoginException()
            : base(Resources.InvalidAccountExceptionMessage)
        {
        }

        public InvalidLoginException(string message)
            : base(message)
        {
        }

        public InvalidLoginException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidLoginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class OutOfServiceException : ServiceException
    {
        public OutOfServiceException()
            : base(Resources.OutOfServiceExceptionMessage)
        {
        }

        public OutOfServiceException(string message)
            : base(message)
        {
        }

        public OutOfServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected OutOfServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
	public class InvalidCreditCardException : ServiceException
	{
		public InvalidCreditCardException()
			: base(Resources.InvalidCreditCardExceptionMessage)
		{
		}

		public InvalidCreditCardException(string message)
			: base(message)
		{
		}

		public InvalidCreditCardException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidCreditCardException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class InvalidEmiratesIdException : ServiceException
	{
		public InvalidEmiratesIdException()
			: base(Resources.InvalidEmiratesIDExceptionMessage)
		{
		}

		public InvalidEmiratesIdException(string message)
			: base(message)
		{
		}

		public InvalidEmiratesIdException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidEmiratesIdException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class InvalidCustomerException : ServiceException
	{
		public InvalidCustomerException()
			: base(Resources.InvalidCustomerExceptionMessage)
		{
		}

		public InvalidCustomerException(string message)
			: base(message)
		{
		}

		public InvalidCustomerException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidCustomerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class InvalidOtpException : ServiceException
	{
		public InvalidOtpException()
			: base(Resources.InvalidOtpExceptionMessage)
		{
		}

		public InvalidOtpException(string message)
			: base(message)
		{
		}

		public InvalidOtpException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidOtpException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class InvalidPinException : ServiceException
	{
		public InvalidPinException() : base(Resources.InvalidPinExceptionMessage)
		{
		}

		public InvalidPinException(string msg) : base(msg)
		{
		}

		public InvalidPinException(string msg, Exception inner) : base(msg, inner)
		{
		}

		protected InvalidPinException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class InvalidTransactionException : ServiceException
	{
		public InvalidTransactionException() : base(Resources.InvalidTransactionExceptionMessage)
		{
		}

		public InvalidTransactionException(string message) : base(message)
		{
		}

		public InvalidTransactionException(string message, Exception inner) : base(message, inner)
		{
		}

		protected InvalidTransactionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class TimeoutException : ServiceException
	{
		public TimeoutException() : base(Resources.TimeoutExceptionMessage)
		{
		}

		public TimeoutException(string message) : base(message)
		{
		}

		public TimeoutException(string message, Exception inner) : base(message, inner)
		{
		}

		protected TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class PrintingFailedException : Exception
	{
		public PrintingFailedException() : base(Resources.RepositoryExceptionMessage)
		{
		}

		public PrintingFailedException(string msg) : base(msg)
		{
		}

		public PrintingFailedException(string msg, Exception innerException) : base(msg, innerException)
		{
		}

		protected PrintingFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

	[Serializable]
	public class EIDCardsNotFoundException : ServiceException
	{
		public EIDCardsNotFoundException()
			: base(Resources.ExceededWithdrawalLimitExceptionMessage)
		{
		}

		public EIDCardsNotFoundException(string message)
			: base(message)
		{
		}

		public EIDCardsNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected EIDCardsNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

    [Serializable]
    public class EmailExistException : ServiceException
    {
        public EmailExistException() : base(Resources.EmailExistExceptionMessage)
        {

        }

        public EmailExistException(string message) : base(message)
        {

        }

        public EmailExistException(string message, Exception inner) : base(message, inner)
        {

        }

        protected EmailExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class UsernameExistException : ServiceException
    {
        public UsernameExistException() : base(Resources.UsernameExistExceptionMessage)
        {

        }

        public UsernameExistException(string message) : base(message)
        {

        }

        public UsernameExistException(string message, Exception inner) : base(message, inner)
        {

        }

        protected UsernameExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class MobileExistException : ServiceException
    {
        public MobileExistException() : base(Resources.MobileExistExceptionMessage)
        {

        }

        public MobileExistException(string message) : base(message)
        {

        }

        public MobileExistException(string message, Exception inner) : base(message, inner)
        {

        }

        protected MobileExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class UsernameCreateException : ServiceException
    {
        public UsernameCreateException() : base(Resources.UserCreateExcetionMessage)
        {

        }

        public UsernameCreateException(string message) : base(message)
        {

        }

        public UsernameCreateException(string message, Exception inner) : base(message, inner)
        {

        }

        protected UsernameCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class UpdateInfoException : ServiceException
    {
        public UpdateInfoException() : base(Resources.UpdateInfoExceptionMessage)
        {

        }

        public UpdateInfoException(string message) : base(message)
        {

        }

        public UpdateInfoException(string message, Exception inner) : base(message, inner)
        {

        }

        protected UpdateInfoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class UpdateEmailException : ServiceException
    {
        public UpdateEmailException() : base(Resources.EmailExistExceptionMessage)
        {

        }

        public UpdateEmailException(string message) : base(message)
        {

        }

        public UpdateEmailException(string message, Exception inner) : base(message, inner)
        {

        }

        protected UpdateEmailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class NoNewUsersException : ServiceException
    {
        public NoNewUsersException() : base(Resources.EmailExistExceptionMessage)
        {

        }

        public NoNewUsersException(string message) : base(message)
        {

        }

        public NoNewUsersException(string message, Exception inner) : base(message, inner)
        {

        }

        protected NoNewUsersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class UpdateMobileException : ServiceException
    {
        public UpdateMobileException() : base(Resources.MobileExistExceptionMessage)
        {

        }

        public UpdateMobileException(string message) : base(message)
        {

        }

        public UpdateMobileException(string message, Exception inner) : base(message, inner)
        {

        }

        protected UpdateMobileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
    [Serializable]
    public class UpdatePasswordException : ServiceException
    {
        public UpdatePasswordException() : base(Resources.PasswordFailExceptionMessage)
        {

        }

        public UpdatePasswordException(string message) : base(message)
        {

        }

        public UpdatePasswordException(string message, Exception inner) : base(message, inner)
        {

        }

        protected UpdatePasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

    }


}