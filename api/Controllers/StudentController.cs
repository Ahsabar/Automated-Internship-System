using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.data;
using api.Dtos;
using api.Dtos.Announcement;
using api.Helpers;
using api.Interfaces;
using api.models;
using api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireStudentUserType")]
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
		private readonly SystemDBContext _context;
		private readonly IAnnouncementRepository _announcementRepository;
		private readonly IApplicationRepository _applicationRepository;
		private readonly IDocumentRepository _documentRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IMapper _userMapper;
		private readonly IMapper _studentMapper;
		private readonly IMapper _announcementMapper;
		private readonly IFileUploadHandler _uploadFile;
		private readonly UserManager<AppUser> _userManager;

		public StudentController(SystemDBContext context, IAnnouncementRepository announcementRepository, IApplicationRepository applicationRepository, IDocumentRepository documentRepository, IMapper announcementMapper, IFileUploadHandler uploadFile, UserManager<AppUser> userManager, IMapper userMapper, IMapper studentMapper, IStudentRepository studentRepository)
		{
			_context = context;
			_announcementRepository = announcementRepository;
			_applicationRepository = applicationRepository;
			_documentRepository = documentRepository;
			_announcementMapper = announcementMapper;
			_uploadFile = uploadFile;
			_userManager = userManager;
			_userMapper = userMapper;
			_studentMapper = studentMapper;
			_studentRepository = studentRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetHomePage()
		{
			var student = await _userManager.GetUserAsync(User);
			var userDto = _userMapper.Map<UserDto>(student);
			return Ok(userDto);
		}

		[HttpGet("announcements")]
		public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

			var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<StudentAnnouncementDto> announcementDtos = await _announcementRepository.GetAllAsyncAsFilteredForStudent(Id, query);

            return Ok(announcementDtos);
        }

		[HttpPost("student/info")]
		public async Task<IActionResult> CreateStudentInfo([FromBody] CreateStudentInfoDto studentInfoDto)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var studentInfo = _studentMapper.Map<StudentInfo>(studentInfoDto);

			studentInfo.StudentId = studentId;

			await _studentRepository.CreateStudentInfoAsync(studentInfo);

			return Ok(studentInfoDto);
		}

		[HttpPut("student/info")]
		public async Task<IActionResult> UpdateStudentInfo([FromBody] UpdateStudentInfoDto studentInfoDto)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			await _studentRepository.UpdateStudentInfoAsync(studentId, studentInfoDto);

			return Ok(studentInfoDto);
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

		[HttpPost("announcement/apply/{id:int}")]
		public async Task<IActionResult> ApplyToAnnouncement([FromRoute] int id, IFormFile cv)
		{
			if (!ModelState.IsValid)
                return BadRequest(ModelState);

			byte[]? fileData = null;

			if(cv != null && cv.Length > 0)
				fileData = await _uploadFile.UploadFileAsync(cv);
			else
				return BadRequest(new {message = "File is missing or empty"});

			var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var applicationExists = await _context.Application
				.AnyAsync(app => app.StudentId == studentId && app.AnnouncementId == id);

			if (applicationExists)
				return Conflict(new { message = "You have already applied for this announcement." });

			var applicationModel = new Application
			{
				AnnouncementId = id,
				StudentId = studentId
			};

            var application = await _applicationRepository.CreateAsync(applicationModel);

			var documentModel = new Document
			{
				ApplicationId = application.Id,
				UserId = studentId,
				FileType = "CV",
				Name = cv.FileName,
				Data = fileData
			};

			await _documentRepository.CreateAsync(documentModel);

			await _documentRepository.CreateApplicationFormAsync(studentId, application.Id);

			var applicationDto = _announcementMapper.Map<ApplicationDto>(applicationModel);

            return CreatedAtAction(nameof(GetById), new { id = applicationDto.Id }, applicationDto);
		}
    }
}