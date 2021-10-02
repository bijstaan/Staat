using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staat.Data;
using Staat.Services;
using Storage.Net.Blobs;
using GroupDocs.Metadata;

namespace Staat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public FileController(IFileService fileService, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
            _fileService = fileService;
        }

        [HttpGet("{pathNamespace}/{hash}")]
        public async Task<IActionResult> Get(string pathNamespace, string hash)
        {
            pathNamespace = pathNamespace.ToLower();
            if (await _fileService.BlobStorage().ExistsAsync($"{pathNamespace}/{hash}"))
            {
                var fileInfo = await _context.File.Where(x => x.Hash == hash && x.Namespace == pathNamespace).FirstAsync();
                var data = await _fileService.BlobStorage().OpenReadAsync($"{pathNamespace}/{hash}");
                return new FileStreamResult(data, fileInfo.MimeType)
                {
                    FileDownloadName = fileInfo.Name,
                    LastModified = fileInfo.UpdatedAt
                };
            }
            return NotFound();
        }
        
        [HttpPost("{pathNamespace}"), Authorize]
        public async Task<List<FileResponse>> Put(string pathNamespace)
        {
            pathNamespace = pathNamespace.ToLower();
            var uploaded = new List<FileResponse>();
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                using var hasher = SHA256.Create();
                await using var stream = file.OpenReadStream();
                using (var metadata = new Metadata(stream))
                {
                    metadata.Sanitize();
                    metadata.Save(stream);
                }
                var hash = Convert.ToBase64String(await hasher.ComputeHashAsync(stream));
                stream.Seek(0, SeekOrigin.Begin);
                await _fileService.BlobStorage().WriteAsync($"{pathNamespace}/{hash}", stream);
                stream.Close();
                uploaded.Add(new FileResponse
                {
                    Path = $"{HttpContext.Request.PathBase.Value}/{pathNamespace}/{hash}",
                    Name = file.Name
                });
                _context.File.Add(new Models.File
                {
                    Name = file.FileName,
                    Hash = hash,
                    Namespace = pathNamespace,
                    MimeType = MimeKit.MimeTypes.GetMimeType(file.FileName)
                });
            }
            await _context.SaveChangesAsync();
            return uploaded;
        }
    }

    public class FileResponse
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}