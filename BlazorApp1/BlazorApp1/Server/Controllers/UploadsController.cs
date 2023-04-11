using BlazorApp1.Client.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using static BlazorApp1.Client.Pages.Index;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : ControllerBase
    {
        private readonly ILogger<UploadsController> _logger;

        public UploadsController(ILogger<UploadsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-7.0&pivots=server#upload-controller
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromForm] IEnumerable<IFormFile> files)
        {
            _logger.LogWarning("Files are uploading...");

            byte[] buffer = new byte[1024*1024];//mb

            foreach (var file in files)
            {
                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                        int readCount;
                        while ((readCount = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            //ToDo: Consider saving data
                        }
                        _logger.LogWarning("File read {File}", file.FileName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "upload failed.");
                    return BadRequest(ex.Message);
                }
            }
            return Ok();
        }
    }
}
