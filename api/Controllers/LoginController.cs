using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
		private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        public LoginController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
		}

		private async Task<AppUser> FindUserByEmail(string email)
		{
			AppUser user = null;

			var domain = email.Split('@')[1].ToLower(); // Split email to get the domain

			if (email == "buketoksuzoglu@iyte.edu.tr")
			{
				user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
			}
			else if (domain == "iyte.edu.tr")
			{
				user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
			}
			else if (domain == "std.iyte.edu.tr")
			{
				user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
			}
			else
			{
				user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email);
			}

			return user;
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			var user = await FindUserByEmail(loginDto.Email.ToLower());

			// var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Email.ToLower());

            if (user == null) return Unauthorized("Username or password incorrect");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username or password incorrect");

            return Ok(
                new NewUserDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
		}
    }
}