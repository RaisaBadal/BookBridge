using System.Text;
using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Persistance.SMTPService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookBridge.Domain.Interfaces;
using BookBridge.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using BookBridge.Persistance.Reflections;
using BookBridge.API.CustomMiddlwares;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped <UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddTransient<SmtpService>();
builder.Services.AddScoped<IUnitOfWorkRepo, UnitOfWork>();
builder.Services.AddScoped<IBorrowRecord, BorrowRecordRepo>();
builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();
builder.Services.AddScoped<IBookCategoryRepo, BookCategoryRepo>();
builder.Services.AddScoped<IBookRepo, BookRepo>();
builder.Services.AddScoped<INotificationRepo, NotificationRepo>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();
/*builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookCategoryService,BookCategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<INotificationService,NotificationService>();
builder.Services.AddScoped<IIdentityService, IdentityServices>();
builder.Services.AddScoped<IReviewService, WishlistService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IStatisticBookRelatedService,StatisticBookRelatedService>();*/
var domainAssemblyServices = Assembly.Load("BookBridge.Application");
builder.Services.AddInjectServices(domainAssemblyServices);

builder.Services.AddLogging(opt => opt.AddConsole());


builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<BookBridgeDb>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(i =>
    {
        i.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5241",
            ValidAudience = "http://localhost:5241",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("65E255FF-F399-42D4-9C7F-D5D08B0EC285"))
        };
    });



builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(BookBridge.Application.Mapper.AutoMapper));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<BookBridgeDb>(
    str =>
    {
        str.UseSqlServer(builder.Configuration.GetConnectionString("BookDb"));
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.Run();
