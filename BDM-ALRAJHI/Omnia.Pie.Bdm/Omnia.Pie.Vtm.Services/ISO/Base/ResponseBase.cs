namespace Omnia.Pie.Vtm.Services.ISO
{
	public interface IResponse
	{
		string ResponseCode { get; set; }
		Error Error { get; set; }
	}

	public abstract class ResponseBase<TResponse> : IResponse
	{
		public string ResponseCode { get; set; }
		public Error Error { get; set; }
		public TResponse Response { get; set; }
	}
}