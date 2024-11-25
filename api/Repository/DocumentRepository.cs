using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.data;
using api.Interfaces;
using api.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Xceed.Words.NET;

namespace api.Repository
{
	public class DocumentRepository : IDocumentRepository
	{
		private readonly SystemDBContext _context;

		public DocumentRepository(SystemDBContext context)
		{
			_context = context;
		}
		public async Task<Document> CreateAsync(Document documentModel)
		{
			await _context.Document.AddAsync(documentModel);
            await _context.SaveChangesAsync();
            return documentModel;
		}

		public async Task<Document?> UpdateAsync(int id, string fileType, string name, byte[] data)
		{
			var existingDocument = await _context.Document.FirstOrDefaultAsync(x => x.ApplicationId == id && x.FileType == fileType);

			if (existingDocument == null)
				return null;

			existingDocument.Name = name;
			existingDocument.Data = data;

			await _context.SaveChangesAsync();

			return existingDocument;
		}

		public async Task<Document?> CreateApplicationFormAsync(string studentId, int applicationId)
		{
			var studentInfo = await _context.StudentInfo.FindAsync(studentId);

			if(studentInfo == null)
				return null;

			string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "ApplicationForm.docx");

    		byte[] data;

			using (var doc = DocX.Load(templatePath))
			{
				// Replace placeholders with actual values
				doc.ReplaceText("«name»", studentInfo.FullName);
				doc.ReplaceText("«email»", studentInfo.Email);
				doc.ReplaceText("«studentClass»", studentInfo.Degree.ToString());
				doc.ReplaceText("«studentNumber»", studentInfo.StudentNo.ToString());
				doc.ReplaceText("«tcNo»", studentInfo.TcNo.ToString());
				doc.ReplaceText("«user_phone»", studentInfo.StudentPhone);
				doc.ReplaceText("«relative_phone»", studentInfo.RelativePhone);
	
				using (var stream = new MemoryStream())
				{
					doc.SaveAs(stream);
					data = stream.ToArray();
				}		
			}

			var document = new Document
			{
				ApplicationId = applicationId,
				Name = $"{studentInfo.FullName}_ApplicationForm.docx",
				FileType = "ApplicationForm",
				Data = data,
				UserId = studentId
			};

			await _context.Document.AddAsync(document);
            await _context.SaveChangesAsync();

			return document;
		}

		public async Task<(byte[] data, string contentType, string fileName)> DownloadFile(int applicationId, string fileType)
		{
			var document = await _context.Document
                .Where(d => d.ApplicationId == applicationId && d.FileType == fileType)
                .FirstOrDefaultAsync() ?? throw new FileNotFoundException("Document not found.");
			
			var provider = new FileExtensionContentTypeProvider();
			if(!provider.TryGetContentType(document.Name, out var contentType))
				contentType = "application/octet-stream"; 

			return (document.Data, contentType, document.Name);
		}
	}
}