using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetapp.Data;
using aspnetapp.Models;
using Microsoft.AspNetCore.Identity;

namespace aspnetapp.Controllers_API_Blazor
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Authorization/getme
        [HttpGet("getMe")]
        public async Task<ActionResult<TokenParams>> GetMe([FromHeader] string token)
        {
            var userToken = await _context.ApplicationUserToken.Where(p => String.Equals(p.Token, token)).Include(p => p.ApplicationUser).FirstOrDefaultAsync();

            if (userToken is null)
            {
                return NotFound();
            }

            var returnUser = new TokenParams();

            returnUser.Role = "Admin";
            returnUser.Avatar = "https://kenh14cdn.com/thumb_w/660/2020/10/17/1-0952-16029340185651467234759.jpg";
            returnUser.Email = userToken.ApplicationUser.Email;
            returnUser.Username = userToken.ApplicationUser.UserName;

            return Ok(returnUser);
        }

        // Post: api/Authorization/login
        [HttpPost("login")]
        public async Task<ActionResult> PostLogin(LoginParams loginParams)
        {
            var user = await _context.ApplicationUser.Where(u => u.Email.Equals(loginParams.Username)).FirstOrDefaultAsync();
            if (user == null)
            {

                return NotFound();
            }

            var isMatch = await _userManager.CheckPasswordAsync(user, loginParams.Password);

            if (isMatch)
            {
                var token = new TokenParams();
                token.Token = Guid.NewGuid().ToString();
                token.Username = user.UserName;
                token.Avatar = "https://kenh14cdn.com/thumb_w/660/2020/10/17/1-0952-16029340185651467234759.jpg";
                token.Email = user.Email;
                token.Role = "Admin";

                // Save token
                var applicationUserToken = new ApplicationUserToken();
                applicationUserToken.Token = token.Token;
                applicationUserToken.ApplicationUserId = user.Id;

                _context.ApplicationUserToken.Add(applicationUserToken);

                await _context.SaveChangesAsync();


                return Ok(token);
            }

            else
            {
                return BadRequest();
            }



        }

        // Post: api/Authorization/login
        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromHeader] string token)
        {
            var userToken = await _context.ApplicationUserToken.Where(p => String.Equals(p.Token, token)).Include(p => p.ApplicationUser).FirstOrDefaultAsync();

            if (userToken is null)
            {
                return NotFound();
            }

            _context.ApplicationUserToken.Remove(userToken);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        public class LoginParams
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class TokenParams
        {
            public string Token { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Avatar { get; set; }
            public string Role { get; set; }

        }

        public class Header
        {
            public string Token { get; set; }

        }

    }


}