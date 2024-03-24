using BooksExchanger.Services.Interfaces;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace BooksExchanger;

/// <summary>
/// Сервис для работы с хранилищем изображений на Яндекс.Диске.
/// </summary>
public class ImageStorageService : IImageStorageService
{
    private static DiskHttpApi s_api;

    private const string _folderName = "Obshajka_Advertisement_Images"; // TODO: change

    /// <summary>
    /// Инициализирует статический экземпляр сервиса Yandex.Disk.
    /// </summary>
    static ImageStorageService()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("YandexDiskSettings");
        var token = config["token"];//"y0_AgAEA7qkJY7BAAkpoAAAAADcsWWswDVBwOCvSB6glBJthBDT9av8wi4";
            // new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()
            // .GetSection("YandexDiskConnectionStrings")["token"];
        s_api = new DiskHttpApi(token);
        CreateImagesDirectoryIfNotExists();
    }
    
    /// <summary>
    /// Загружает изображение и возвращает ссылку на него.
    /// </summary>
    /// <param name="image">Файл изображения.</param>
    /// <returns>Ссылку на загруженное изображение.</returns>
    public async Task<string> UploadImageAndGetLink(IFormFile image)
    {
        string pathToImage = MakePathToImage(Path.GetExtension(image.FileName));
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var link = await s_api.Files.GetUploadLinkAsync(pathToImage, overwrite: true);
            await s_api.Files.UploadAsync(link, ms);
        }

        var linkForImage = await s_api.Files.GetDownloadLinkAsync(pathToImage);
        return linkForImage.Href;
    }

    private string MakePathToImage(string extention)
    {
        return $"/{_folderName}/{Guid.NewGuid()}.{extention}";
    }

    private static async void CreateImagesDirectoryIfNotExists()
    {
        var rootFolderData = await s_api.MetaInfo.GetInfoAsync(new ResourceRequest
        {
            Path = "/"
        });

        if (!rootFolderData.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(_folderName)))
        {
            await s_api.Commands.CreateDictionaryAsync("/" + _folderName);
        }
    }
}