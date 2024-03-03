namespace BooksExchanger.Models;

/// <summary>
/// Класс, содержащий константы.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Внешний ключ книги к жанру.
    /// </summary>
    public const string BooksGenreFK = "books_genre_id_fkey";
    
    /// <summary>
    /// Внешний ключ книги к авторам.
    /// </summary>
    public const string AuthorsBooksAuthorIdFK = "authors_books_author_id_fkey";
    
    /// <summary>
    /// Внешний ключ оффера к пользователю.
    /// </summary>
    public const string OffersUsersPK = "offers_users_pkey";

    /// <summary>
    /// Уникальное имя в жанрах.
    /// </summary>
    public const string GenresNameKey = "genres_name_key";
    
    /// <summary>
    /// Внешний ключ отзыва к книге.
    /// </summary>
    public const string FeedbacksBookIdFK = "feedbacks_book_id_fkey";
    
    /// <summary>
    /// Внешний ключ оффера к книге.
    /// </summary>
    public const string OffersBookIdFK = "offers_book_id_fkey";
    
    /// <summary>
    /// Максимальное количество офферов в подборке.
    /// </summary>
    public const int MaxOffersInSelection = 5;
}