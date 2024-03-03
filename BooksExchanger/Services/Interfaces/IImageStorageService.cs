namespace BooksExchanger.Services.Interfaces;

public interface IImageStorageService
{
    public Task<string> UploadImageAndGetLink(IFormFile image);
}