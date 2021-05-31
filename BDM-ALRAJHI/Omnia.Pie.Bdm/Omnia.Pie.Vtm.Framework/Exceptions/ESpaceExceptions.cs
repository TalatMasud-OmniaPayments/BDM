namespace Omnia.Pie.Vtm.Framework.Exceptions
{
	using System;

	[Serializable]
	public class LoginFailureException : Exception
	{
		public LoginFailureException(string error) : base(error)
		{

		}

		public LoginFailureException(string error, Exception innerException) : base(error, innerException)
		{

		}
	}

	[Serializable]
	public class CallFailureException : Exception
	{
		public CallFailureException(string error) : base(error)
		{

		}

		public CallFailureException(string error, Exception innerException) : base(error, innerException)
		{

		}
	}
}