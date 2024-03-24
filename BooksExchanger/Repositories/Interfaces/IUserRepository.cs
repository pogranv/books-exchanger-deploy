using BooksExchanger.Models;

namespace BooksExchanger.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория пользователей.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получает пользователя по его электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <returns>Модель пользователя, если пользователь существует; иначе null.</returns>
    public User? GetUserByEmail(string email);

    /// <summary>
    /// Пытается добавить нового пользователя в систему.
    /// </summary>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Модель созданного пользователя.</returns>
    /// <remarks>Метод генерирует исключение, если пользователь с такой электронной почтой уже существует.</remarks>
    public User TryAddNewUser(string name, string email, string password);

    /// <summary>
    /// Назначает пользователю роль администратора по его электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <remarks>Метод генерирует исключение, если пользователь с указанной электронной почтой не найден.</remarks>
    public void SetAdminByEmail(string email);

    /// <summary>
    /// Проверяет, существует ли пользователь по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>True, если пользователь существует; иначе false.</returns>
    public bool IsUserExist(long userId);
}