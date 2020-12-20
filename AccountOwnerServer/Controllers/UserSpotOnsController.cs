using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotOnAccountServer.Dtos;
using SpotOnAccountServer.Models;
using SpotOnAccountServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotOnAccountServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSpotOnsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocUserDTO>>> GetDoctors(int page, int pageSize)
        {
            return await SpotOnRepo.FindPaged<DocUserDTO>(page, pageSize);
        }

        [HttpGet]
        [Route("GetSpecialDoctors")]
        public async Task<ActionResult<IEnumerable<DocUserDTO>>> GetSpecialDoctors(string special, int page, int pageSize)
        {
            return await SpotOnRepo.FindSpecialPaged<DocUserDTO>(special, page, pageSize);
        }

        [HttpPost]
        [Route("PostUsers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> PostUsers(string firstname, string lastname, string emailaddress, string loginname, string password, string stateoforigin, string localgovt)
        {
            Users usser = new Users()
            {
                StateOrigin = stateoforigin,
                EmailAddress = emailaddress,
                FirstName = firstname,
                LastName = lastname,
                LoginName = loginname,
                Password = password,
                LocalGovt = localgovt
            };
            return await SpotOnRepo.AddLoginAsync<DocUserDTO>(usser, "");
        }

        [HttpPost]
        [Route("PostSpecialists")]
        public async Task<ActionResult<IEnumerable<DocUserDTO>>> PostSpecialists(int userId, string specialization, string languages)
        {
            Specialists usser = new Specialists()
            {
                UserId = userId,
                Specialization = specialization,
                Language = languages
            };
            return await SpotOnRepo.AddDocAsync<Specialists>(usser);
        }


        [HttpGet("authenticate")]
        public IActionResult Authenticate(string username, string password)
        {
            Users usser = new Users()
            {
                LoginName = username,
                Password = password
            };

            var user = SpotOnRepo.AuthenticateLogins<Users>(usser);

            if (user == null || user.FirstOrDefault().FirstName == "Invalid")
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost("setuserbusy/{id}")]
        public IActionResult SetUserBusy(int id)
        {
            try
            {
                // save 
                var _user = SpotOnRepo.SetUserBusy(id);
                return Ok(_user);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }



        [AllowAnonymous]
        [HttpPost("passwordrecovery")]
        public IActionResult PasswordRecovery(string email)
        {
            var mail = MailService.SendMail(email);
            return Ok(mail);
        }

        [AllowAnonymous]
        [HttpPost("confirmcode")]
        public IActionResult ConfirmCode(string email, string code)
        {
            var Confirm = MailService.ConfirmCode(email, code);

            return Ok(Confirm);
        }


        [AllowAnonymous]
        [HttpPut("passwordchange")]
        public IActionResult PasswordChange(string email, string password)
        {
            var Confirm = SpotOnRepo.UpdatePassword(email, password);
            return Ok(Confirm);

        }

    }
}
