namespace webapp.Services
{
    public class BufferedFileUpload : IBufferedFileUpload
    {
        public string FolderPath { get; set; } = Path.Combine(
            Environment.GetEnvironmentVariable("DataDirectory")
            ?? Environment.CurrentDirectory, "DataDirectory");

        public async Task<bool> UploadFile(IFormFile file)
        {
            try
            {
                if(file.Length == 0)
                {
                    return false;
                }
                
                string destination = Path.GetFullPath(FolderPath);
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }

                using var fileStream = new FileStream(Path.Combine(destination, file.FileName), FileMode.Create);
                await fileStream.CopyToAsync(fileStream);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public interface IBufferedFileUpload
    {
        Task<bool> UploadFile(IFormFile file);
    }
}