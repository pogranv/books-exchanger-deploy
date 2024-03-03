using System.ComponentModel;

namespace BooksExchanger.Models;

/// <summary>
/// Статус модерации.
/// </summary>
public enum ModerationStatus
{
    /// <summary>
    /// Отправлен.
    /// </summary>
    [Description("submitted")]
    Submitted,
    
    /// <summary>
    /// На рассмотрении.
    /// </summary>
    [Description("consideration")]
    Consideration,
    
    /// <summary>
    /// Подтвержден.
    /// </summary>

    [Description("approved")]
    Approved,
    
    /// <summary>
    /// Отклонен.
    /// </summary>

    [Description("rejected")]
    Rejected
}