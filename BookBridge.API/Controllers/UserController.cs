using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.response;
using BookBridge.Application.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BookBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IIdentityService identityService) : ControllerBase
    {
        private readonly IIdentityService identityService = identityService;

        [HttpPost]
        [Route("User/[action]")]
        public async Task<Response<IdentityResult>> Registration(UserModel userModel)
        {
            try
            {
                if(!ModelState.IsValid) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
                var res=await identityService.Registration(userModel);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<IdentityResult>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User/[action]")]
        public async Task<Response<SignInResult>> SignIn(SignInModel signInModel)
        {
            try
            {
                if(!ModelState.IsValid) return Response<SignInResult>.Error(ErrorKeys.BadRequest);
                var res= await identityService.SignIn(signInModel);
                return Response<SignInResult>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<SignInResult>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route(nameof(SignOutNow))]
        public async Task<Response<bool>> SignOutNow()
        {
            try
            {
                if (User.Identity is null || !User.Identity.IsAuthenticated || User.Identity.Name is null)
                    return Response<bool>.Error(ErrorKeys.BadRequest);
                var res = await identityService.SignOut();
                return Response<bool>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<bool>.Error(e.Message,e.StackTrace,ErrorKeys.BadRequest);
            }
        }

        [HttpGet]
        [Route("User/[action]")]
        public async Task<Response<UserModel>> Info()
        {
            try
            {

                if (User?.Identity is not { Name: not null, IsAuthenticated: true })
                    return Response<UserModel>.Error(ErrorKeys.BadRequest);
                var res = await identityService.Info(User.Identity.Name);
                return Response<UserModel>.Ok(res);
            }
            catch (Exception e)
            {
               return Response<UserModel>.Error(e.Message,e.StackTrace,ErrorKeys.InternalServerError);
            }
        }
    }
}
