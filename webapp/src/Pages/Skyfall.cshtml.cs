using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using webapp.Services;

namespace webapp.Pages
{
    public class SkyfallModel : PageModel
    {
        private readonly IBufferedFileUpload bufferedFileUpload;
        private readonly ILogger<SkyfallModel> logger;

        public SkyfallModel(IBufferedFileUpload bufferedFileUpload, ILogger<SkyfallModel> logger)
        {
            this.bufferedFileUpload = bufferedFileUpload;
            this.logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(IFormFile file)
        {
            using (logger.BeginScope(nameof(OnPost)))
            {
                try
                {
                    logger.LogInformation("Attempting to upload file {0} ({1} KB)",
                                          file.Name,
                                          file.Length / 1000);
                    if (await bufferedFileUpload.UploadFile(file))
                    {
                        return RedirectToPage("SkyFall");
                    }
                    else
                    {
                        Trace.TraceWarning("{0} could not be uploaded", file.Name);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("The following exception occurred when trying to upload the file. \n\r {0}", ex.ToString());
                }

                return BadRequest();
            }
        }

    }
}
