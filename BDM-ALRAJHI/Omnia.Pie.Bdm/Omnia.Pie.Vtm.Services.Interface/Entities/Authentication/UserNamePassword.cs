
namespace Omnia.Pie.Vtm.Services.Interface.Entities
{
   public class UserNamePassword
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerIdentifier { get; set; }
        public string IccResponse { get; set; }
        public string AuthCode { get; set; }
        public string ResponseCode { get; set; }
    }
}
