using System.ComponentModel;

namespace BooksExchanger.Entities;

/// <summary>
/// Объект роли пользователя в БД.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    [Description("user")]
    User,
    
    /// <summary>
    /// Роль админа.
    /// </summary>
    [Description("admin")]
    Admin
}