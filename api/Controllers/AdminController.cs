using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminUserType")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
		private readonly SystemDBContext _context;
		private readonly IAnnouncementRepository _announcementRepository;
		private readonly IApplicationRepository _applicationRepository;
		private readonly IDocumentRepository _documentRepository;
		private readonly IFileUploadHandler _uploadFile;
		private readonly IMapper _announcementMapper;
		private readonly IMapper _applicationMapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _userMapper;
		public AdminController(SystemDBContext context, IAnnouncementRepository announcementRepository, IApplicationRepository applicationRepository, IDocumentRepository documentRepository, IFileUploadHandler uploadFile, IMapper announcementMapper, UserManager<AppUser> userManager, IMapper userMapper, IMapper applicationMapper)
		{
			_context = context;
			_announcementRepository = announcementRepository;
			_applicationRepository = applicationRepository;
			_documentRepository = documentRepository;
			_uploadFile = uploadFile;
			_announcementMapper = announcementMapper;
			_userManager = userManager;
			_userMapper = userMapper;
			_applicationMapper = applicationMapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetHomePage()
		{
			var admin = await _userManager.GetUserAsync(User);
			var userDto = _userMapper.Map<UserDto>(admin);
			return Ok(userDto);
		}

		[HttpGet("companyRequests")]
		public async Task<IActionResult> GetAllCompanies()
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcementDtos = await _announcementRepository.GetAllAsyncAsFilteredForAdmin();

            return Ok(announcementDtos);
		}

		[HttpGet("announcements")]
		public async Task<IActionResult> GetAll()
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcementDtos = await _announcementRepository.GetAllAsyncAsFilteredForAdmin();

            return Ok(announcementDtos);
		}

		[HttpPut("announcement/{id:int}")]
		public async Task<IActionResult> ApproveAnnouncement([FromRoute] int id, [FromBody] ApproveDto approveDto)
		{
			// need to add email
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = await _announcementRepository.ApproveAsync(id, approveDto);

            if (message == null)
                return NotFound();

            return Ok(message);
		}

		[HttpGet("applications")]
		public async Task<IActionResult> GetApplications([FromQuery] QueryObject query)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AdminApplicationDto> applicationDtos = await _applicationRepository.GetAllAsyncForAdmin(query);

            return Ok(applicationDtos);
		}

		[HttpPut("application/{id:int}")]
		public async Task<IActionResult> ApproveApplication([FromRoute] int id, [FromForm] ApproveDto approveDto, IFormFile file)
		{
			// need to add email
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			byte[]? fileData = null;

			if(file != null && file.Length > 0)
				fileData = await _uploadFile.UploadFileAsync(file);
			else
				return BadRequest(new {message = "File is missing or empty"});

            var result = await _applicationRepository.ApproveApplicationAsyncForAdmin(id, approveDto);
		
			if (result == null)
                return NotFound();

			var (application, message) = result.Value;

			var document = await _documentRepository.UpdateAsync(application.Id, "ApplicationForm", file.FileName, fileData);

			if(document == null)
				return NotFound("Document couldn't be updated");

            return Ok(message);
		}

		[HttpGet("download/{applicationId}/{fileType}")]
		public async Task<IActionResult> DownloadFile(int applicationId, string fileType)
		{
			var (data, contentType, fileName) = await _documentRepository.DownloadFile(applicationId, fileType);

			return File(data, contentType, fileName);
		}
    }
}