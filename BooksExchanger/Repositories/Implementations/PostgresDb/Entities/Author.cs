using System;
using System.Collections.Generic;

namespace BooksExchanger.Entities;

/// <summary>
/// Класс автора в БД.
/// </summary>
public partial class Author
{
    /// <summary>
    /// Id автора.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Связанные книги.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
