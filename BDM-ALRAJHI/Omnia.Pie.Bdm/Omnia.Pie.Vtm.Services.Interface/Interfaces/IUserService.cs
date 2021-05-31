using System;


namespace Omnia.Pie.Vtm.Services.Interface
{
    using Omnia.Pie.Vtm.Services.Interface.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IUserService
    {
        Task<UserInfo> GetUserInformationAsync(string userName, string AccountName);
        Task<UpdatePassword> UpdatePasswordAsync(string userName,  string oldPassword, string newPassword);
        Task<UserInfo> UpdateUserInformationAsync(string userName, string AccountName, string name, string email, string mobile, string userType);
        Task<UserInfo> ValidatePasswordAsync(string UserName, string Password);
        //Task<UserInfo> RegisterFingerprintAsync(string Username, string Fingerprint);
        Task<UserInfo> RegisterFingerprintAsync(string Username, string Fingerprint, string FingerIndex);
        Task<UserInfo> ValidateFingerprintAsync(string Username, string Fingerprint);
        Task<List<UserInfo>> GetAllUsersAsync(string userName);
        Task<List<UserTypes>> GetUserTypesAsync(string userName);
        Task<List<UserInfo>> GetNewUsersAsync(string userName, string lastUserId);

    }
}
