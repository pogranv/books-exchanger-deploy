using System.ComponentModel;

namespace BooksExchanger.Controllers.Specs.Common;

/// <summary>
/// Модель статусов модерации оффера.
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