using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Authorization.Handlers;
using api.Dtos;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [ApiController]
    [Route("api/register")]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;

		public RegisterController(UserManager<AppUser> userManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		[HttpPost("student")]
		public async Task<IActionResult> RegisterStudent([FromBody] RegisterDto registerDto)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);

				var Student = new Student
				{
					UserName = registerDto.Email,
					Email = registerDto.Email,
					UserType = "Student"
				};

				var createdUser = await _userManager.CreateAsync(Student, registerDto.Password);

				if(createdUser.Succeeded)
				{
					return Ok(
						new NewUserDto
						{
							FullName = Student.FullName,
							Email = Student.Email,
							Token = _tokenService.CreateToken(Student)
						}
					);
				}
				else 
				{
					return StatusCode(500, createdUser.Errors);
				}

			}
			catch (Exception e)
			{
				return StatusCode(500, e);
			}
		}

		[HttpPost("company")]
		public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyDto registerCompanyDto)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);

				var Company = new Company
				{
					UserName = registerCompanyDto.Email,
					Email = registerCompanyDto.Email,
					Name = registerCompanyDto.Name,
            		Address = registerCompanyDto.Address,
					FullName = registerCompanyDto.FullName,
					UserType = "Company"
				};

				var createdUser = await _userManager.CreateAsync(Company, registerCompanyDto.Password);

				if(createdUser.Succeeded)
				{
					return Ok(
						new NewUserDto
						{
							FullName = Company.FullName,
							Email = Company.Email,
							Token = _tokenService.CreateToken(Company)
						}
					);
				}
				else 
				{
					return StatusCode(500, createdUser.Errors);
				}

			}
			catch (Exception e)
			{
				return StatusCode(500, e);
			}
		}

		/*[HttpPost("admin")]
		public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);

				var Admin = new AppUser
				{
					FullName = "Buket Öksüzoğlu",
					UserName = registerDto.Email,
					Email = registerDto.Email,
					UserType = "Admin"
				};

				var createdUser = await _userManager.CreateAsync(Admin, registerDto.Password);

				if(createdUser.Succeeded)
				{
					return Ok("Admin Created");
				}
				else 
				{
					return StatusCode(500, createdUser.Errors);
				}

			}
			catch (Exception e)
			{
				return StatusCode(500, e);
			}
		}

		[HttpPost("secretary")]
		public async Task<IActionResult> RegisterSecretary([FromBody] RegisterDto registerDto)
		{
			try
			{
				if(!ModelState.IsValid)
					return BadRequest(ModelState);

				var Admin = new AppUser
				{
					FullName = "Mehmet Anıl Cömert",
					UserName = registerDto.Email,
					Email = registerDto.Email,
					UserType = "Secretary"
				};

				var createdUser = await _userManager.CreateAsync(Admin, registerDto.Password);

				if(createdUser.Succeeded)
				{
					return Ok("Secretary Created");
				}
				else 
				{
					return StatusCode(500, createdUser.Errors);
				}

			}
			catch (Exception e)
			{
				return StatusCode(500, e);
			}
		}*/
    }
}