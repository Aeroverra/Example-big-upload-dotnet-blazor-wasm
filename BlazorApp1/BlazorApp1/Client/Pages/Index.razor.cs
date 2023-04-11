using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorApp1.Client.Pages
{
    public partial class Index
    {

        [Inject] private ILogger<Index> Logger { get; set; } = null!;
        [Inject] private HttpClient HttpClient { get; set; } = null!;
        private long _maxFileSize = (1024L * 1024 * 1024 * 15); //15gb
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
  



            foreach (var file in e.GetMultipleFiles())
            {
                using (var content = new MultipartFormDataContent())
                {
                    try
                    {
                        Logger.LogInformation("{FileName} {Size} has started uploading.", file.Name, file.Size);

                        var fileContent = new StreamContent(file.OpenReadStream(_maxFileSize));

                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                        content.Add(content: fileContent, name: "\"files\"", fileName: file.Name);

                        var response = await HttpClient.PostAsync("/Uploads", content);

                        if (response.IsSuccessStatusCode)
                        {
                            using var responseStream = await response.Content.ReadAsStreamAsync();
                        }
                        Logger.LogInformation("{FileName} {Size} has finished uploading.", file.Name, file.Size);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "{FileName} not uploaded {Message}", file.Name, ex.Message);
                    }
                }
            }


        }

        private async Task OnInputFileChange2(InputFileChangeEventArgs e)
        {

            foreach (var file in e.GetMultipleFiles())
            {

                Logger.LogInformation("{FileName} {Size} has started uploading.", file.Name, file.Size);

                byte[] buffer = new byte[1024 * 1024];//mb

                using (var stream = file.OpenReadStream(_maxFileSize))
                {
                    int readCount;
                    while ((readCount = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        //ToDo: Consider using data
                    }

                }

                Logger.LogInformation("{FileName} {Size} has finished uploading.", file.Name, file.Size);

            }
        }
    }
}