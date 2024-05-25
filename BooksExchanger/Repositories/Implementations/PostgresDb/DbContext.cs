using Microsoft.EntityFrameworkCore;

using Npgsql;

using BooksExchanger.Entities;

namespace BooksExchanger.Context;

/// <summary>
/// Контекст БД.
/// </summary>
public partial class DbCtx : DbContext
{
    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static DbCtx()
     {
         NpgsqlConnection.GlobalTypeMapper.MapEnum<ModerationStatus>();
         NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRole>();
     }
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    public DbCtx()
    {
    }
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="options"></param>

    public DbCtx(DbContextOptions<DbCtx> options)
        : base(options)
    {
    }

    /// <summary>
    /// Авторы.
    /// </summary>
    public virtual DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Книги.
    /// </summary>
    public virtual DbSet<Book> Books { get; set; }

    /// <summary>
    /// Чаты.
    /// </summary>
    public virtual DbSet<Chat> Chats { get; set; }

    /// <summary>
    /// Отзывы.
    /// </summary>
    public virtual DbSet<Feedback> Feedbacks { get; set; }

    /// <summary>
    /// Жанры.
    /// </summary>
    public virtual DbSet<Genre> Genres { get; set; }
    
    /// <summary>
    /// Сообщения.
    /// </summary>

    public virtual DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Офферы.
    /// </summary>
    public virtual DbSet<Offer> Offers { get; set; }

    /// <summary>
    /// Офферы на модрации.
    /// </summary>
    public virtual DbSet<OffersCollector> OffersCollectors { get; set; }

    /// <summary>
    /// Пользователи.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        // => optionsBuilder.UseNpgsql("Host=localhost;Port=5555;Database=books_exchanger;Username=postgres;Password=andrew7322");
        => optionsBuilder.UseNpgsql("Server=postgres_db;Port=5432;User id=postgres;password=andrew7322;database=books_exchanger");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("moderation_status", new[] { "submitted", "consideration", "approved", "rejected" })
            .HasPostgresEnum("request_status", new[] { "open", "in_progress", "closed" })
            .HasPostgresEnum("user_role", new[] { "admin", "user" });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("authors_pkey");

            entity.ToTable("authors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasMany(d => d.Books).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorsBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("authors_books_book_id_fkey"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("authors_books_author_id_fkey"),
                    j =>
                    {
                        j.HasKey("AuthorId", "BookId").HasName("authors_books_pkey");
                        j.ToTable("authors_books");
                        j.IndexerProperty<long>("AuthorId").HasColumnName("author_id");
                        j.IndexerProperty<long>("BookId").HasColumnName("book_id");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountRating)
                .HasDefaultValue(0L)
                .HasColumnName("count_rating");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.SumRating)
                .HasDefaultValue(0L)
                .HasColumnName("sum_rating");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("books_genre_id_fkey");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("chats_pkey");

            entity.ToTable("chats");

            entity.HasIndex(e => new { e.User1Id, e.User2Id }, "chats_user1_id_user2_id_key").IsUnique();

            entity.HasIndex(e => new { e.User1Id, e.User2Id }, "idx_user_pairs").IsUnique();

            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.User1Id).HasColumnName("user1_id");
            entity.Property(e => e.User2Id).HasColumnName("user2_id");

            entity.HasOne(d => d.User1).WithMany(p => p.ChatUser1s)
                .HasForeignKey(d => d.User1Id)
                .HasConstraintName("chats_user1_id_fkey");

            entity.HasOne(d => d.User2).WithMany(p => p.ChatUser2s)
                .HasForeignKey(d => d.User2Id)
                .HasConstraintName("chats_user2_id_fkey");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feedbacks_pkey");

            entity.ToTable("feedbacks");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Estimation).HasColumnName("estimation");
            entity.Property(e => e.Feedback1).HasColumnName("feedback");
            entity.Property(e => e.GivenByUserId).HasColumnName("given_by_user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("feedbacks_book_id_fkey");

            entity.HasOne(d => d.GivenByUser).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.GivenByUserId)
                .HasConstraintName("feedbacks_given_by_user_id_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.Name, "genres_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("messages_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sent_at");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("messages_chat_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("messages_user_id_fkey");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("offers_pkey");

            entity.ToTable("offers");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Picture).HasColumnName("picture");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");

            entity.HasOne(d => d.Book).WithMany(p => p.Offers)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("offers_book_id_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.Offers)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("offers_owner_id_fkey");
        });

        modelBuilder.Entity<OffersCollector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("offers_collector_pkey");

            entity.ToTable("offers_collector");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Authors).HasColumnName("authors");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Picture).HasColumnName("picture");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.RejectReason)
                .HasMaxLength(200)
                .HasColumnName("reject_reason");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Owner).WithMany(p => p.OffersCollectors)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("offers_collector_owner_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Salt)
                .HasColumnName("salt");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");

            entity.HasMany(d => d.OffersNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "OffersUser",
                    r => r.HasOne<Offer>().WithMany()
                        .HasForeignKey("OfferId")
                        .HasConstraintName("offers_users_offer_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("offers_users_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "OfferId").HasName("offers_users_pkey");
                        j.ToTable("offers_users");
                        j.IndexerProperty<long>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<Guid>("OfferId").HasColumnName("offer_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
