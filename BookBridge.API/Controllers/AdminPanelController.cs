using BookBridge.Application.Interfaces;
using BookBridge.Application.Models.Request;
using BookBridge.Application.response;
using BookBridge.Application.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookBridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
   // [Authorize]
    public class AdminPanelController(IIdentityService identityServices) : ControllerBase
    {
        private readonly IIdentityService _identityServices = identityServices;

        [HttpPost]
        [Route("Role/{roleName}/[action]")]
        public async Task<Response<IdentityResult>> AddRole([FromRoute] string roleName)
        {
            try
            {
                if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
                var res = await _identityServices.CreateRole(roleName);
                return res
                    ? Response<IdentityResult>.Ok(IdentityResult.Success)
                    : Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception e)
            {
                return Response<IdentityResult>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Role/{roleName}/[action]")]
        public async Task<Response<IdentityResult>> DeleteRole([FromRoute] string roleName)
        {
            try
            {
                if (!ModelState.IsValid) throw new ArgumentException(ErrorKeys.BadRequest);
                var res = await _identityServices.DeleteRole(roleName);
                return res
                    ? Response<IdentityResult>.Ok(IdentityResult.Success)
                    : Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception e)
            {
                return Response<IdentityResult>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<string>>> Roles()
        {
            try
            {
                var res = await _identityServices.GetAllRole();
                return Response<IEnumerable<string>>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<string>>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User/{roleName:alpha}[action]/{userId}")]
        public async Task<Response<IdentityResult>> AssignRoleToUser([FromRoute] string roleName,
            [FromRoute] string userId)
        {
            try
            {
                if (!ModelState.IsValid) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
                var res = await _identityServices.AssignRoleToUser(roleName, userId);
                return res
                    ? Response<IdentityResult>.Ok(IdentityResult.Success)
                    : Response<IdentityResult>.Error(ErrorKeys.BadRequest);
            }
            catch (Exception e)
            {
                return Response<IdentityResult>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Response<IEnumerable<UserModel>>> AllUser()
        {
            try
            {
                var res= await _identityServices.GetAllUser();
                return Response<IEnumerable<UserModel>>.Ok(res);
            }
            catch (Exception e)
            {
                return Response < IEnumerable < UserModel >>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User/{userId}/[action]")]
        public async Task<Response<UserModel>> UserById([FromRoute]string userId)
        {
            try
            {
                if(!ModelState.IsValid) return Response<UserModel>.Error(ErrorKeys.BadRequest);
                var res=await _identityServices.GetUserById(userId);
                return Response<UserModel>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<UserModel>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }

        [HttpPost]
        [Route("User/{id}/[action]")]
        public async Task<Response<IdentityResult>> Delete(string id)
        {
            try
            {
                if(!ModelState.IsValid) return Response<IdentityResult>.Error(ErrorKeys.BadRequest);
                var res = await _identityServices.DeleteUser(id);
                return Response<IdentityResult>.Ok(res);
            }
            catch (Exception e)
            {
                return Response<IdentityResult>.Error(e.Message, e.StackTrace, ErrorKeys.InternalServerError);
            }
        }


    }
}
