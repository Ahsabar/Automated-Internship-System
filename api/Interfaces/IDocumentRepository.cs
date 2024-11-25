using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> CreateAsync(Document documentModel);
		Task<Document?> UpdateAsync(int id, string fileType, string name, byte[] data);
		Task<Document?> CreateApplicationFormAsync(string studentId, int applicationId);
		Task<(byte[] data, string contentType, string fileName)> DownloadFile(int applicationId, string fileType);
    }
}