using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Services
{
    using Omnia.Pie.Vtm.Bootstrapper.Interface;
    using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Services.Interface;
    using Omnia.Pie.Vtm.Services.Interface.Entities;
    using Omnia.Pie.Vtm.Services.Interface.Interfaces;
    using Omnia.Pie.Vtm.Services.ISO.Authentication;
    using Omnia.Pie.Vtm.Services.ISO.UserManagement;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    

    public class UserService : ServiceBase, IUserService
    {

        public UserService(IResolver container, IServiceEndpoint endpointsProvider) : base(container, endpointsProvider)
        {
            
        }

        #region "Get User Information"

        public async Task<UserInfo> GetUserInformationAsync(string userName, string AccountName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(AccountName)) throw new ArgumentNullException(nameof(AccountName));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<UserInfoRequest, UserInfo>(async c =>
            {
                UserInfoRequest request = ToDocumentVTMRetrieveUserInfoRequest(userName, AccountName);
                var response = await GetUserInformationAsync(request);
                return ToUserInfo(response);
            });
        }

        private async Task<UserInfoResponse> GetUserInformationAsync(UserInfoRequest request)
            => await ExecuteServiceAsync<UserInfoRequest, UserInfoResponse>(request);

        private UserInfoRequest ToDocumentVTMRetrieveUserInfoRequest(string userName, string AccountName) => new UserInfoRequest
        {
            Username = userName,
            UserAccount = GetUpdateAccountRequest(AccountName),
        };

        private UpdateAccount GetUpdateAccountRequest(string AccountName) => new UpdateAccount
        {
            Username = AccountName,
        };

        private UserInfo ToUserInfo(UserInfoResponse response) => new UserInfo
        {
            Name = response?.UserInfo?.Name,
            Username = response?.UserInfo?.Username,
            Email = response?.UserInfo?.Email,
            Mobile = response?.UserInfo?.Mobile,
            UserType = response?.UserInfo?.UserType,
        };

        #endregion

        #region "Get All Users"
        // Omnia.Pie.Vtm.Services.Interface.Entities
        public async Task<List<Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo>> GetAllUsersAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<GetAllUsersRequest, List<Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo>>(async c =>
            {
                GetAllUsersRequest request = ToDocumentVTMRetrieveAllUsersRequest(userName);
                var response = await GetAllUsersAsync(request);
                //var allUsers = ToAllUsers(response);
                //return ToAllUsersInfo(allUsers.UserInfo);
                return response.UserInfo;
            });
        }

        private async Task<GetAllUserInfoResponse> GetAllUsersAsync(GetAllUsersRequest request)
            => await ExecuteServiceAsync<GetAllUsersRequest, GetAllUserInfoResponse>(request);

        private GetAllUsersRequest ToDocumentVTMRetrieveAllUsersRequest(string userName) => new GetAllUsersRequest
        {
            Username = userName
        };

        private GetAllUserInfoResponse ToAllUsers(GetAllUserInfoResponse response) => new GetAllUserInfoResponse
        {
            //var allUsers = new List<UserInfo>();
            ResponseCode = response?.ResponseCode,
            UserInfo = response?.UserInfo
            //UserInfo = response.userInfo.UserInfo,

        };
        


        #endregion

        #region "Update User Information"

        public async Task<UserInfo> UpdateUserInformationAsync(string userName, string AccountName, string name, string email, string mobile, string userType)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(AccountName));
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(mobile)) throw new ArgumentNullException(nameof(mobile));
            if (string.IsNullOrEmpty(mobile)) throw new ArgumentNullException(nameof(userType));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<UserInfoRequest, UserInfo>(async c =>
            {
                UserInfoRequest request = ToDocumentVTMRetrieveUserInfoRequest(userName, AccountName, name, email, mobile, userType);
                var response = await UpdateUserInformationAsync(request);
                return ToUserInfo(response);
            });
        }

        private async Task<UserInfoResponse> UpdateUserInformationAsync(UserInfoRequest request)
            => await ExecuteServiceAsync<UserInfoRequest, UserInfoResponse>(request);

        private UserInfoRequest ToDocumentVTMRetrieveUserInfoRequest(string userName, string AccountName, string name, string email, string mobile, string userType) => new UserInfoRequest
        {
            Username = userName,
            UserAccount = UpdateAccountRequest(AccountName, name, email, mobile, userType),
        };

        private UpdateAccount UpdateAccountRequest(string  AccountName, string name, string email, string mobile, string userType) => new UpdateAccount
        {
            Username = AccountName,
            Name = name,
            Email = email,
            Mobile = mobile,
            UserType = userType,
        };

        #endregion

        #region "Update Password"
        public async Task<UpdatePassword> UpdatePasswordAsync(string userName,  string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(oldPassword)) throw new ArgumentNullException(nameof(oldPassword));
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentNullException(nameof(newPassword));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<UpdatePasswordRequest, UpdatePassword>(async c =>
            {
                UpdatePasswordRequest request = ToDocumentVTMUpdatePasswordRequest(userName,oldPassword, newPassword);
                var response = await UpdatePasswordAsync(request);
                return ToUpdatePassword(response);
            });
        }
        private UpdatePasswordRequest ToDocumentVTMUpdatePasswordRequest(string userName, string oldPassword, string newPassword) => new UpdatePasswordRequest
        {
            Username = userName,
            OldPassword = oldPassword,
            NewPassword = newPassword,
        };
        private async Task<UpdatePasswordResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
           => await ExecuteServiceAsync<UpdatePasswordRequest, UpdatePasswordResponse>(request);

        private UpdatePassword ToUpdatePassword(UpdatePasswordResponse response) => new UpdatePassword
        {
            Name = response?.updatePassword?.Name,
            Username = response?.updatePassword?.Username,
            Email = response?.updatePassword?.Email,
            Mobile = response?.updatePassword?.Mobile,
            UserType = response?.updatePassword?.UserType,
            ResponseCode = response?.updatePassword?.ResponseCode,
        };
        #endregion

        #region UserName Password Verification

        public async Task<UserInfo> ValidatePasswordAsync(string userName, string password)
        {

            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<UserNamePasswordRequest, UserInfo>(async c =>
            {
                UserNamePasswordRequest request = ToDocumentVTMUserNamePasswordRequest(userName, password);
                var response = await ValidatePasswordAsync(request);
                return ToUpdateUserNamePassword(response);
            });
        }
        private UserNamePasswordRequest ToDocumentVTMUserNamePasswordRequest(string userName, string password) => new UserNamePasswordRequest
        {
            Username = userName,
            Password = password,
        };
        private async Task<UserInfoResponse> ValidatePasswordAsync(UserNamePasswordRequest request)
           => await ExecuteServiceAsync<UserNamePasswordRequest, UserInfoResponse>(request);
       
        private UserInfo ToUpdateUserNamePassword(UserInfoResponse response) => new UserInfo
        {
            Username = response?.UserInfo?.Username,
            UserType  = response?.UserInfo?.UserType,
            Password = response?.UserInfo?.Password,
            Name = response?.UserInfo?.Name,
            Email= response?.UserInfo?.Email,
            Mobile = response?.UserInfo?.Mobile,
            ResponseCode = response?.ResponseCode,

        };
        #endregion
        #region "Get New Users"
        // Omnia.Pie.Vtm.Services.Interface.Entities
        public async Task<List<UserInfo>> GetNewUsersAsync(string userName, string lastUserId)
        {
            if (string.IsNullOrEmpty(lastUserId)) throw new ArgumentNullException(nameof(lastUserId));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<GetAllUsersRequest, List<Omnia.Pie.Vtm.Services.Interface.Entities.UserInfo>>(async c =>
            {
                GetNewUsersRequest request = ToDocumentVTMRetrieveNewUsersRequest(userName, lastUserId);
                var response = await GetNewUsersAsync(request);
                var allnewUsers = ToNewUsers(response);
                //return ToAllUsersInfo(allUsers.UserInfo);
                return response.NewUsers;
            });
        }

        private async Task<GetNewUsersResponse> GetNewUsersAsync(GetNewUsersRequest request)
            => await ExecuteServiceAsync<GetNewUsersRequest, GetNewUsersResponse>(request);

        private GetNewUsersRequest ToDocumentVTMRetrieveNewUsersRequest(string userName, string lastUserId) => new GetNewUsersRequest
        {
            Username = "",
            LastUserId = lastUserId
        };

        private GetNewUsersResponse ToNewUsers(GetNewUsersResponse response) => new GetNewUsersResponse
        {
            //var allUsers = new List<UserInfo>();
            ResponseCode = response?.ResponseCode,
            NewUsers = response?.NewUsers
            //UserInfo = response.userInfo.UserInfo,

        };
        #endregion
        #region "Register Fingerprints"
        public async Task<UserInfo> RegisterFingerprintAsync(string Username, string Fingerprint, string FingerIndex)
        {
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));
            if (string.IsNullOrEmpty(Fingerprint)) throw new ArgumentNullException(nameof(Fingerprint));
            if (string.IsNullOrEmpty(FingerIndex)) throw new ArgumentNullException(nameof(FingerIndex));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<RegisterFingerprintRequest, UserInfo>(async c =>
            {
                RegisterFingerprintRequest request = ToDocumentBDMRegisterFingerprintRequest(Username, Fingerprint, FingerIndex);
                var response = await RegisterFingerprintAsync(request);
                return ToRegisterFingerprint(response);
            });
        }
        private RegisterFingerprintRequest ToDocumentBDMRegisterFingerprintRequest(string Username, string Fingerprint, string FingerIndex) => new RegisterFingerprintRequest
        {
            Username = Username,
            UserAccount = GetUserAccountRequest(Username, Fingerprint, FingerIndex),
        };
        private UserAccount GetUserAccountRequest(string Username, string Fingerprint, string FingerIndex) => new UserAccount
        {
            Username = Username,
            Fingerprint = Fingerprint,
            FingerIndex = FingerIndex,
        };
        private async Task<UserInfoResponse> RegisterFingerprintAsync(RegisterFingerprintRequest request)
           => await ExecuteServiceAsync<RegisterFingerprintRequest, UserInfoResponse>(request);

        private UserInfo ToRegisterFingerprint(UserInfoResponse response) => new UserInfo
        {
            Name = response?.UserInfo?.Name,
            Username = response?.UserInfo?.Username,
            Email = response?.UserInfo?.Email,
            Mobile = response?.UserInfo?.Mobile,
            UserType = response?.UserInfo?.UserType,
            ResponseCode = response?.ResponseCode,
        };
        #endregion


        #region "Validate Fingerprints"
        public async Task<UserInfo> ValidateFingerprintAsync(string Username, string Fingerprint)
        {
            if (string.IsNullOrEmpty(Username)) throw new ArgumentNullException(nameof(Username));
            if (string.IsNullOrEmpty(Fingerprint)) throw new ArgumentNullException(nameof(Fingerprint));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<ValidateFingerprintRequest, UserInfo>(async c =>
            {
                ValidateFingerprintRequest request = ToDocumentBDMValidateFingerprintRequest(Username, Fingerprint);
                var response = await ValidateFingerprintAsync(request);
                return ValidateFingerprintAsync(response);
            });
        }
        private ValidateFingerprintRequest ToDocumentBDMValidateFingerprintRequest(string Username, string Fingerprint) => new ValidateFingerprintRequest
        {
            Username = Username,
            Fingerprint = Fingerprint,
        };
        private async Task<UserInfoResponse> ValidateFingerprintAsync(ValidateFingerprintRequest request)
           => await ExecuteServiceAsync<ValidateFingerprintRequest, UserInfoResponse>(request);

        private UserInfo ValidateFingerprintAsync(UserInfoResponse response) => new UserInfo
        {
            Name = response?.UserInfo?.Name,
            Username = response?.UserInfo?.Username,
            Email = response?.UserInfo?.Email,
            Mobile = response?.UserInfo?.Mobile,
            UserType = response?.UserInfo?.UserType,
            ResponseCode = response?.ResponseCode,
        };
        #endregion


        #region "Get User Types"
        // Omnia.Pie.Vtm.Services.Interface.Entities
        public async Task<List<Omnia.Pie.Vtm.Services.Interface.Entities.UserTypes>> GetUserTypesAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
            //var account = UserInfoRequest(AccountName);
            return await ExecuteFaultHandledOperationAsync<GetUserTypesRequest, List<Omnia.Pie.Vtm.Services.Interface.Entities.UserTypes>>(async c =>
            {
                GetUserTypesRequest request = ToDocumentVTMRetrieveUserTypesRequest(userName);
                var response = await GetUserTypesAsync(request);
                var allUserTypes = ToUserTypes(response);
                //return ToAllUsersInfo(allUsers.UserInfo);
                return allUserTypes;
            });
        }

        private async Task<GetUserTypesResponse> GetUserTypesAsync(GetUserTypesRequest request)
            => await ExecuteServiceAsync<GetUserTypesRequest, GetUserTypesResponse>(request);

        private GetUserTypesRequest ToDocumentVTMRetrieveUserTypesRequest(string userName) => new GetUserTypesRequest
        {
            Username = userName
        };

        /*private GetUserTypesResponse ToUserTypes(GetUserTypesResponse response) => new GetUserTypesResponse
        {
            ResponseCode = response?.ResponseCode,
            UserTypes = response?.UserTypes

        };*/
        private List<Omnia.Pie.Vtm.Services.Interface.Entities.UserTypes> ToUserTypes(GetUserTypesResponse response)
        {
            var types = new List<Omnia.Pie.Vtm.Services.Interface.Entities.UserTypes>();
            for (int i = 0; i < response?.UserTypes.Count; i++)
            {
                var userType = new Omnia.Pie.Vtm.Services.Interface.Entities.UserTypes
                {
                    Type = response?.UserTypes[i]
                };
                
                types.Add(userType);
            }
            return types;
        }
       




        #endregion
    }

}
