namespace UserProfileService.Shared.Services.attachmentServices
{
    public interface IattachmentService
    {
        string? UploadImage(IFormFile file, string folderName);

        bool DeleteImage(string imagePath);

    }
}
