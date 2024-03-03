namespace BooksExchanger.Models;

/// <summary>
/// Класс-маппер моделей.
/// </summary>
public class ResponseMapper
{

    /// <summary>
    /// Маппит модели автора.
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Author MapAuthor(Author author)
    {
        return new Controllers.Specs.Common.Author
        {
            Id = author.Id,
            Name = author.Name
        };
    }
    
    /// <summary>
    /// Маппит модели автора.
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public Author MapAuthor(Entities.Author author)
    {
        return new Author
        {
            Id = author.Id,
            Name = author.Name
        };
    }

    /// <summary>
    /// Маппит модели статуса модерации.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.ModerationStatus MapModerationStatus(ModerationStatus status)
    {
        switch (status)
        {
            case ModerationStatus.Approved:
                return Controllers.Specs.Common.ModerationStatus.Approved;
            case ModerationStatus.Consideration:
                return Controllers.Specs.Common.ModerationStatus.Consideration;
            case ModerationStatus.Rejected:
                return Controllers.Specs.Common.ModerationStatus.Rejected;
            case ModerationStatus.Submitted:
                return Controllers.Specs.Common.ModerationStatus.Submitted;
        }

        return Controllers.Specs.Common.ModerationStatus.Submitted;
    }
    
    /// <summary>
    /// Маппит модели статуса модерации.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public ModerationStatus MapModerationStatus(Entities.ModerationStatus status)
    {
        switch (status)
        {
            case Entities.ModerationStatus.Approved:
                return ModerationStatus.Approved;
            case Entities.ModerationStatus.Consideration:
                return ModerationStatus.Consideration;
            case Entities.ModerationStatus.Rejected:
                return ModerationStatus.Rejected;
            case Entities.ModerationStatus.Submitted:
                return ModerationStatus.Submitted;
        }

        return ModerationStatus.Submitted;
    }
    
    /// <summary>
    /// Маппит модели статуса модерации.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public Entities.ModerationStatus MapModerationStatusToDb(ModerationStatus status)
    {
        switch (status)
        {
            case ModerationStatus.Approved:
                return Entities.ModerationStatus.Approved;
            case ModerationStatus.Consideration:
                return Entities.ModerationStatus.Consideration;
            case ModerationStatus.Rejected:
                return Entities.ModerationStatus.Rejected;
            case ModerationStatus.Submitted:
                return Entities.ModerationStatus.Submitted;
        }

        return Entities.ModerationStatus.Submitted;
    }
    
    /// <summary>
    /// Маппит модели офферов на модерации.
    /// </summary>
    /// <param name="offerCollector"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.OfferCollector MapOfferCollector(OfferCollector offerCollector)
    {
        return new Controllers.Specs.Common.OfferCollector()
        {
            Id = offerCollector.Id,
            Authors = offerCollector.Authors,
            City = offerCollector.City,
            CreatedAt = offerCollector.CreatedAt.ToString(),
            Description = offerCollector.Description,
            ModerationStatus = MapModerationStatus(offerCollector.ModerationStatus),
            Owner = MapOwner(offerCollector.Owner),
            Picture = offerCollector.Picture,
            Price = offerCollector.Price,
            RegectReason = offerCollector.RegectReason,
            Title = offerCollector.Title
        };
    }
    
    /// <summary>
    /// Маппит модели офферов на модерации.
    /// </summary>
    /// <param name="offersCollector"></param>
    /// <returns></returns>
    public OfferCollector MapOfferCollector(Entities.OffersCollector offersCollector)
    {
        return new OfferCollector()
        {
            Id = offersCollector.Id,
            Authors = offersCollector.Authors,
            City = offersCollector.City,
            CreatedAt = offersCollector.CreatedAt,
            Description = offersCollector.Description,
            ModerationStatus = MapModerationStatus(offersCollector.ModerationStatus),
            Owner = MapOwner(offersCollector.Owner),
            Picture = offersCollector.Picture,
            Price = offersCollector.Price,
            RegectReason = offersCollector.RejectReason,
            Title = offersCollector.Title
        };
    }
    
    /// <summary>
    /// Маппит модели чатов.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    public ChatWithMessages MapChatWithMessages(Entities.Chat chat, long currentUserId)
    {
        return new ChatWithMessages()
        {
           Id = chat.ChatId,
           Messages = chat.Messages.ToList().ConvertAll(MapMessage),
           UserId = currentUserId == chat.User1Id ? chat.User2Id : chat.User1Id,
           UserName = currentUserId == chat.User1Id ? chat.User2.Name : chat.User1.Name
        };
    }
    
    /// <summary>
    /// Маппит модели сообщений.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Message MapMessage(Message message, long currentUserId)
    {
        return new Controllers.Specs.Common.Message()
        {
            Id = message.Id,
            IsUserSender = message.UserId == currentUserId,
            SentAt = message.SentAt.ToString(),
            Text= message.Text,
            UserId = message.UserId
        };
    }
    
    /// <summary>
    /// Маппит модели сообщений.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Message MapMessage(Entities.Message message)
    {
        return new Message()
        {
            Id = message.MessageId,
            SentAt = message.SentAt,
            Text = message.Text,
            UserId = message.UserId.Value
        };
    }
    
    /// <summary>
    /// Маппит модели чатов.
    /// </summary>
    /// <param name="chat"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Chat MapChat(Chat chat)
    {
        return new Controllers.Specs.Common.Chat
        {
            Id = chat.Id,
            LastMessage = chat.LastMessage,
            UserName = chat.UserName,
            UserId = chat.UserId
        };
    }

    /// <summary>
    /// Маппит модели чатов.
    /// </summary>
    /// <param name="chat"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    public Chat MapChat(Entities.Chat chat, long currentUserId)
    {
        var userName = chat.User1.Id == currentUserId ? chat.User2.Name : chat.User1.Name;
        var userId = chat.User1.Id == currentUserId ? chat.User2.Id : chat.User1.Id;
        string? lastMessage = null;
        if (chat.Messages.Count != 0)
        {
            lastMessage = chat.Messages.First().Text;
        }
        return new Chat
        {
            Id = chat.ChatId,
            LastMessage = lastMessage,
            UserId = userId,
            UserName = userName
        };
    }
    
    /// <summary>
    /// Маппит модели жанров.
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Genre MapGenre(Genre genre)
    {
        return new Controllers.Specs.Common.Genre
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
    
    /// <summary>
    /// Маппит модели жанров.
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    public Genre MapGenre(Entities.Genre genre)
    {
        return new Genre
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
    
    /// <summary>
    /// Маппит модели владельца.
    /// </summary>
    /// <param name="owner"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Owner MapOwner(Owner owner)
    {
        return new Controllers.Specs.Common.Owner
        {
            Id = owner.Id,
            Name = owner.Name
            
        };
    }
    
    /// <summary>
    /// Маппит модели владельца.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Owner MapOwner(Entities.User user)
    {
        return new Owner()
        {
            Id = user.Id,
            Name = user.Name
        };
    }
    
    /// <summary>
    /// Маппит модели офферов.
    /// </summary>
    /// <param name="offer"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Offer MapOffer(Offer offer)
    {
        return MapOffer(offer, false);
    }
    
    /// <summary>
    /// Маппит модели офферов.
    /// </summary>
    /// <param name="offer"></param>
    /// <param name="isFavorite"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Offer MapOffer(Offer offer, bool isFavorite = false)
    {
        return new Controllers.Specs.Common.Offer
        {
            Id = offer.Id,
            Authors = offer.Authors.ConvertAll(MapAuthor),
            BookRating = offer.BookRating,
            City = offer.City,
            CreatedAt = offer.CreatedAt,
            Description = offer.Description,
            Genre = MapGenre(offer.Genre),
            IsFavoriteForUser = isFavorite,
            Owner = MapOwner(offer.Owner),
            Picture = offer.Picture,
            Price = offer.Price,
            Title = offer.Title
        };
    }
    
    /// <summary>
    /// Маппит модели офферов.
    /// </summary>
    /// <param name="offer"></param>
    /// <returns></returns>
    public Offer MapOffer(Entities.Offer offer)
    {
        decimal? bookRating = null;
        if (offer.Book.CountRating != 0)
        {
            bookRating = (decimal)offer.Book.SumRating / offer.Book.CountRating;
        }
        return new Offer
        {
            Id = offer.Id,
            Authors = offer.Book.Authors.ToList().ConvertAll(MapAuthor),
            BookRating = bookRating,
            City = offer.City,
            CreatedAt = offer.CreatedAt.ToString(),
            Description = offer.Description,
            Genre = MapGenre(offer.Book.Genre),
            Owner = MapOwner(offer.Owner),
            Picture = offer.Picture,
            Price = offer.Price,
            Title = offer.Book.Title,
        };
    }
    
    /// <summary>
    /// Маппит модели книг.
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Book MapBook(Book book)
    {
        return new Controllers.Specs.Common.Book
        {
            Id = book.Id,
            Title = book.Title,
            Authors = book.Authors.ConvertAll(MapAuthor),
            Genre = MapGenre(book.Genre)
        };
    }
    
    /// <summary>
    /// Маппит модели книг.
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    public Book MapBook(Entities.Book book)
    {
        return new Book
        {
            Id = book.Id,
            Authors = book.Authors.ToList().ConvertAll(MapAuthor),
            Genre = MapGenre(book.Genre),
            Title = book.Title
        };
    }
    
    /// <summary>
    /// Маппит модели подборок.
    /// </summary>
    /// <param name="selection"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Selection MapSelection(Selection selection)
    {
        return new Controllers.Specs.Common.Selection
        {
            Title = selection.Title,
            Offers = selection.Offers.ConvertAll(MapOffer)
        };
    }
    
    /// <summary>
    /// Маппит модели отзывов.
    /// </summary>
    /// <param name="feedback"></param>
    /// <returns></returns>
    public Controllers.Specs.Common.Feedback MapFeedback(Models.Feedback feedback)
    {
        return new Controllers.Specs.Common.Feedback
        {
            CreatedAt = feedback.CreatedAt.ToString(),
            Estimation = feedback.Estimation,
            Id = feedback.Id,
            UserName = feedback.UserName,
            Value = feedback.Text
        };
    }
    
    /// <summary>
    /// Маппит модели отзывов.
    /// </summary>
    /// <param name="feedback"></param>
    /// <returns></returns>
    public Models.Feedback MapFeedback(Entities.Feedback feedback, BookEstimation bookEstimation)
    {
        return new Models.Feedback
        {
            Id = feedback.Id,
            CreatedAt = feedback.CreatedAt,
            Estimation = feedback.Estimation,
            GivenByUserId = feedback.GivenByUserId,
            BookEstimation = bookEstimation,
            UserName = feedback.GivenByUser.Name,
            Text = feedback.Feedback1,
        };
    }
    
    /// <summary>
    /// Маппит модели юзеров.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Models.User? MapUser(Entities.User? user)
    {
        if (user == null)
        {
            return null;
        }
        return new Models.User
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role == Entities.UserRole.User ? Models.UserRole.User : Models.UserRole.Admin
        };
    }
}