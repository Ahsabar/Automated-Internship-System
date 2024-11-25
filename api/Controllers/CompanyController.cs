using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
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
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireCompanyUserType")]
    [ApiController]
    [Route("api/company")]
    public class CompanyController : ControllerBase
    {
		private readonly SystemDBContext _context;
		private readonly IMapper _userMapper;
		private readonly IMapper _announcementMapper;
		private readonly IAnnouncementRepository _announcementRepository;
		private readonly IApplicationRepository _applicationRepository;
		private readonly IDocumentRepository _documentRepository;
		private readonly IFileUploadHandler _uploadFile;
		private readonly IImageUploadHandler _uploadImage;
		private readonly UserManager<AppUser> _userManager;

		public CompanyController(SystemDBContext context, IMapper userMapper, IMapper announcementMapper, IAnnouncementRepository announcementRepository, IApplicationRepository applicationRepository, IImageUploadHandler uploadImage, IFileUploadHandler uploadFile, IDocumentRepository documentRepository, UserManager<AppUser> userManager)
		{
			_context = context;
			_userMapper = userMapper;
			_announcementMapper = announcementMapper;
			_announcementRepository = announcementRepository;
			_applicationRepository = applicationRepository;
			_uploadImage = uploadImage;
			_uploadFile = uploadFile;
			_documentRepository = documentRepository;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetHomePage()
		{
			var company = await _userManager.GetUserAsync(User);
			var userDto = _userMapper.Map<UserDto>(company);
			return Ok(userDto);
		}

		[HttpGet("announcements")]
		public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Announcement> announcements = await _announcementRepository.GetAllAsync(query);

            List<AnnouncementDto> announcementDtos = _announcementMapper.Map<List<AnnouncementDto>>(announcements);

            return Ok(announcementDtos);
        }

		[HttpGet("announcement/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcement = await _announcementRepository.GetByIdAsync(id);

            if (announcement == null)	
                return NotFound();
        
            return Ok(_announcementMapper.Map<AnnouncementDto>(announcement));
        }

		[HttpPost("announcement")]
		public async Task<IActionResult> CreateAnnouncement([FromForm] CreateAnnouncementDto createAnnouncementDto, IFormFile? image)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			byte[]? fileData = null;

			if(image != null && image.Length > 0)
				fileData = await _uploadImage.UploadFileAsync(image);
			
			var announcement = _announcementMapper.Map<Announcement>(createAnnouncementDto);

			announcement.CompanyId = companyId;
			announcement.Image = fileData;

            await _announcementRepository.CreateAsync(announcement);

			var announcementDto = _announcementMapper.Map<AnnouncementDto>(announcement);

            return CreatedAtAction(nameof(GetById), new { id = announcementDto.Id }, announcementDto);
		}

		[HttpPut("announcement/{id:int}")]
		public async Task<IActionResult> UpdateAnnouncement([FromRoute] int id, [FromForm] UpdateAnnouncementDto updateAnnouncementDto, IFormFile? image)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			byte[]? fileData = null;

			if(image != null && image.Length > 0) 
				fileData = await _uploadImage.UploadFileAsync(image);
		
			updateAnnouncementDto.Image = fileData;

            var announcementModel = await _announcementRepository.UpdateAsync(id, updateAnnouncementDto);

            if (announcementModel == null)
                return NotFound();

			var announcementDto = _announcementMapper.Map<AnnouncementDto>(announcementModel);

            return Ok(announcementDto);
		}

		[HttpGet("applications")]
		public async Task<IActionResult> GetApplications([FromQuery] QueryObject query)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AdminApplicationDto> applicationDtos = await _applicationRepository.GetAllAsyncForCompany(query);

            return Ok(applicationDtos);
		}

		[HttpGet("application/{id:int}")]
		public async Task<IActionResult> GetApplicationById([FromRoute] int id)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var applicationDto = await _applicationRepository.GetByIdAsync(id);

			if (applicationDto == null)	
                return NotFound();

            return Ok(applicationDto);
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

            var result = await _applicationRepository.ApproveApplicationAsyncForCompany(id, approveDto);

			if (result == null)
				return BadRequest();

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