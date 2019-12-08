using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using WebApplication11.Models;
using WebApplication11.Service;
using WebApplication11.ViewModel;

namespace WebApplication11.Controllers
{
   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private readonly IHttpContextAccessor _httpContext;


        public AuthController(IUserService userService, IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _httpContext = httpContext;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginModels userParam)
        {
            try
            {
                var user = _userService.Authenticate(userParam.Email, EncryptionUtility.Encrypt(userParam.Password));

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }

        [AllowAnonymous]
        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody]UserViewModel model)
        {
            try
            {
                var data = Mapper.Map<UserProfile>(model);

                data.Password = EncryptionUtility.Encrypt(data.Password);

                var resp = _userService.Adduser(data);

                if (resp == false)
                    return BadRequest(new { message = "Email already taken" });

                return Ok("User Added Successfully! Kindly Proceed to login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpGet("GetUser")]
        public IActionResult Getuser()
        {
            try
            {
               
                int id = 0;

                var hhhj = _httpContext.HttpContext.User.Claims.ToList().Find(r => r.Type == "id").Value;

                if (_httpContext.HttpContext.User.Claims.ToList().Find(r => r.Type == "id") != null)
                {
                    string userId = _httpContext.HttpContext.User.Claims.ToList().Find(r => r.Type == "id").Value;
                    id = Convert.ToInt32(userId);
                }

                var resp = _userService.GetUser(id);

                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

      
    }
}