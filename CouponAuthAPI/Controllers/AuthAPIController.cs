using CouponAuthAPI.Model;
using CouponAuthAPI.Repository;
using CouponAuthAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Controllers;

namespace CouponAuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IJWT _jWT;
        public AuthAPIController(UserRepository userRepository,IJWT jWT)
        {
            _userRepository = userRepository;
            _jWT = jWT;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(ApplicationUser applicationUser)
        {
            try
            {
                if(applicationUser.UserName == null || applicationUser.UserEmail == null || applicationUser.UserPassword == null)
                {
                    return BadRequest("Fill all data");
                }
                _userRepository.UserRegister(applicationUser);
            }
            catch(Exception ex)
            {

            }
            return Ok(new { messahe = "User Created Successfully", IsSuccess = true});
        }
        [HttpPost("login")]
        public IActionResult LogIn(string userEmail, string userPassword)
        {

            var user = _userRepository.LogInCoupon(userEmail, userPassword)?.FirstOrDefault();
            if (user == null)
            {
                return BadRequest(new { message = "User Email or password is incorrect" });
            }

            var token = _jWT.GenerateToken(user);
            return Ok(new { user, token, message = "All data is here" });
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> LogAssign([FromBody] RoleAssigning roleAssigning)
        {
            try
            {
                if(roleAssigning.UserName == null || roleAssigning.UserEmailId == null || roleAssigning.UserPassWord == null)
                {
                    return BadRequest("Fill all data");
                }
                _userRepository.RollAssign(roleAssigning);
            }
            catch(Exception ex)
            {

            }
            return Ok(new { message = "Role assigned for", roleAssigning.UserName});
        }

    }
}
