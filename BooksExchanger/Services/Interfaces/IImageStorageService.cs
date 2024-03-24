namespace BooksExchanger.Services.Interfaces;

/// <summary>
/// Определяет интерфейс для службы хранения изображений, позволяющей загружать изображения и получать ссылки на них.
/// </summary>
public interface IImageStorageService
{
    /// <summary>
    /// Загружает изображение и возвращает ссылку на загруженное изображение.
    /// </summary>
    /// <param name="image">Файл изображения для загрузки.</param>
    /// <returns>Задача, результатом которой является строка, содержащая ссылку на загруженное изображение.</returns>
    public Task<string> UploadImageAndGetLink(IFormFile image);
}