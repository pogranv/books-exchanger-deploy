using System.ComponentModel.DataAnnotations.Schema;

namespace BooksExchanger.Entities;

/// <summary>
/// Объект оффера до модерации в БД.
/// </summary>
public partial class OffersCollector
{
    /// <summary>
    /// id оффера.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// id владельца.
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Авторы книги.
    /// </summary>
    public string Authors { get; set; } = null!;

    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Цена.
    /// </summary>
    public decimal? Price { get; set; }
    
    /// <summary>
    /// Статус модерации.
    /// </summary>
    [Column("status", TypeName = "moderation_status")]
     public ModerationStatus ModerationStatus { get; set; }

    /// <summary>
    /// Город размещения.
    /// </summary>
    public string City { get; set; } = null!;

    /// <summary>
    /// Прицина отказа.
    /// </summary>
    public string? RejectReason { get; set; }

    /// <summary>
    /// Ссылка на картинку.
    /// </summary>
    public string? Picture { get; set; }

    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время удаления.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Информация о владельце.
    /// </summary>
    public virtual User? Owner { get; set; }
}
