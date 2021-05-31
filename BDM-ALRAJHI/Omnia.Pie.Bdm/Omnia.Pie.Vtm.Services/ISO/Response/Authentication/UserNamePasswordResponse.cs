namespace Omnia.Pie.Vtm.Services.ISO.Authentication
{
    using System.Collections.Generic;

    public class UserNamePasswords
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserType { get; set; }
    }

    public class UserNamePasswordResponse : ResponseBase<UserNamePasswords>
    {
        public UserNamePasswords userNamePassword{ get; set; }
    }
}