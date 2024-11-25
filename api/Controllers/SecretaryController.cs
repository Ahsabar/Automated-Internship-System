using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireSecretaryUserType")]
    [ApiController]
    [Route("api/secretary")]
    public class SecretaryController : ControllerBase
    {
		private readonly IApplicationRepository _applicationRepository;
		private readonly IDocumentRepository _documentRepository;
		private readonly IInternshipRepository _internshipRepository;
		private readonly IFileUploadHandler _uploadFile;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _userMapper;

		public SecretaryController(IApplicationRepository applicationRepository, UserManager<AppUser> userManager, IMapper userMapper, IDocumentRepository documentRepository, IFileUploadHandler uploadFile, IInternshipRepository internshipRepository)
		{
			_applicationRepository = applicationRepository;
			_userManager = userManager;
			_userMapper = userMapper;
			_documentRepository = documentRepository;
			_uploadFile = uploadFile;
			_internshipRepository = internshipRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetHomePage()
		{
			var secretary = await _userManager.GetUserAsync(User);
			var userDto = _userMapper.Map<UserDto>(secretary);
			return Ok(userDto);
		}
		
		[HttpGet("applications")]
		public async Task<IActionResult> GetApplications([FromQuery] QueryObject query)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AdminApplicationDto> applicationDtos = await _applicationRepository.GetAllAsyncForSecretary(query);

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

		[HttpGet("download/{applicationId}/{fileType}")]
		public async Task<IActionResult> DownloadFile([FromRoute] int applicationId, [FromRoute] string fileType)
		{
			var (data, contentType, fileName) = await _documentRepository.DownloadFile(applicationId, fileType);

			return File(data, contentType, fileName);
		}

		[HttpPost("application/{applicationId}")]
		public async Task<IActionResult> PostEmploymentCertificate([FromRoute] int applicationId, [FromForm] ApproveDto approveDto, IFormFile file)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			byte[]? fileData = null;

			if(file != null && file.Length > 0)
				fileData = await _uploadFile.UploadFileAsync(file);
			else
				return BadRequest(new {message = "File is missing or empty"});

            var result = await _applicationRepository.ApproveApplicationAsyncForSecretary(applicationId, approveDto);

			if (result == null)
				return BadRequest();

			var (application, message) = result.Value;

			var document = new Document()
			{
				ApplicationId = application.Id,
				UserId = application.Student.Id,
				FileType = "EmploymentCertificate",
				Name = file.FileName,
				Data = fileData
			};

			var internship = new Internship()
			{
				Id = application.Id,
				StudentId = application.Student.Id,
				StudentName = application.Student.StudentInfo.FullName
			};
			
			await _internshipRepository.CreateAsync(internship);
			
			await _documentRepository.CreateAsync(document);

            return Ok(message);
		}
	}
}