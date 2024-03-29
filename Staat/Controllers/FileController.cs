﻿/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
using File = Staat.Data.Models.File;

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
                _context.File.Add(new File
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