using System.Reflection;
using BooksExchanger;
// using BooksExchanger.Hubs;
using BooksExchanger.MetanitHub;
// using BooksExchanger.MetanitHub;
using BooksExchanger.Middlewares;
using BooksExchanger.Repositories.Implementations;
using BooksExchanger.Repositories.Interfaces;
using BooksExchanger.Services.Implementations.AuthorService;
using BooksExchanger.Services.Implementations.BookService;
using BooksExchanger.Services.Implementations.ChatService;
using BooksExchanger.Services.Implementations.FeedbackService;
using BooksExchanger.Services.Implementations.GenreService;
using BooksExchanger.Services.Implementations.OffersCollectorService;
using BooksExchanger.Services.Implementations.OfferService;
using BooksExchanger.Services.Implementations.UserService;
using BooksExchanger.Services.Interfaces;
using BooksExchanger.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Obshajka.VerificationCodesManager;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
        builder.WithOrigins("*")  // Замените "http://example.com" адресом вашего фронтенд приложения
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
#region Swagger Configuration
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo { });
    
    swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    // Включаем фильтр для обработки перечислений с атрибутами Description
    swagger.AddEnumsWithValuesFixFilters();

    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.RequireHttpsMetadata = false;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidIssuer = AuthOptions.ISSUER,
//             ValidateAudience = true,
//             ValidAudience = AuthOptions.AUDIENCE,
//             ValidateLifetime = true,
//             IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//             ValidateIssuerSigningKey = true,
//         };
//         // options.Events = new JwtBearerEvents
//         // {
//         //     OnMessageReceived = context =>
//         //     {
//         //         // context.HttpContext.Items[""]
//         //         var accessToken = context.Request.Query["access_token"];
//         //
//         //         // если запрос направлен хабу
//         //         var path = context.HttpContext.Request.Path;
//         //         if (!string.IsNullOrEmpty(accessToken) &&
//         //             (path.StartsWithSegments("/chat")))
//         //         {
//         //             // получаем токен из строки запроса
//         //             context.Token = accessToken;
//         //         }
//         //         return Task.CompletedTask;
//         //     }
//         // };
//     });

builder.Services.AddSignalR();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IOffersCollectorService, OffersCollectorService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IOffersCollectorRepository, OffersCollectorRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
// builder.Services.AddSingleton<IVerificationCodesManager, VerificationCodesManager>();

var app = builder.Build();
app.UseCors("MyCorsPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapHub<ChatHub>("/chat");
// });
// app.MapHub<CommunicationHub>("/chat");
app.MapHub<ChatHub>("/chat");

app.Run();