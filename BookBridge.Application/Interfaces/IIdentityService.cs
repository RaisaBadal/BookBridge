using BookBridge.Application.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace BookBridge.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResult> Registration(UserModel userModel);
        Task<SignInResult> SignIn(SignInModel signInModel);
        Task<bool> SignOut();
        Task<bool>CreateRole(string  roleName);
        Task<bool> DeleteRole(string roleName);
        Task<IEnumerable<string>> GetAllRole();
        Task<bool> AssignRoleToUser(string roleName,string userId);
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<UserModel> GetUserById(string userId);
        Task<IdentityResult> DeleteUser(string id);
        Task<UserModel> Info(string Username);

    }
}
