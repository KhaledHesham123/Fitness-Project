
namespace UserProfileService.Shared.Services.attachmentServices
{
    public class attachmentService : IattachmentService
    {
        List<string> allowedExtensions = new List<string> { ".jpg", ".png", ".jpeg" };
        const int maxsize = 2097152; 

       

        public string? UploadImage(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension)) { return null; }


            if (file.Length > maxsize || file.Length == 0) return null;


            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            var FileName = $"{Guid.NewGuid()}_{file.FileName}";

            var filePath = Path.Combine(FolderPath, FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("Files",folderName,FileName).Replace("\\", "/");

        }


        public bool DeleteImage(string imagePath)
        {
            if (!File.Exists(imagePath)) return false;
            else
            {
                File.Delete(imagePath);
                return true;
            }

        }
    }
}
