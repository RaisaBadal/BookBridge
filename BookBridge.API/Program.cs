using System.Reflection;
using System.Text;
using BookBridge.Domain.Data;
using BookBridge.Domain.Entities;
using BookBridge.Persistance.SMTPService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookBridge.Application.Interfaces;
using BookBridge.Domain.Interfaces;
using BookBridge.Infrastructure.Repositories;
using BookBridge.Application.Services;
using BookBridge.Persistance.Reflections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookCategoryService,BookCategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IIdentityService, IdentityServices>();
builder.Services.AddScoped<IReviewService, WishlistService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();


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


#region MyRegion

//var domainAssembly = Assembly.Load("BookBridge.Domain");
//var infrastructureAssembly = Assembly.Load("BookBridge.Infrastructure");
//builder.Services.AddInjectRepositories(infrastructureAssembly);

//builder.Services.AddInjectServices(domainAssembly);

//var apLoad = Assembly.Load("BookBridge.Application");
//builder.Services.AddInjectRepositories(apLoad);


#endregion

builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(BookBridge.Application.Mapper.AutoMapper));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<BookBridgeDb>(
    str =>
    {
        str.UseSqlServer(builder.Configuration.GetConnectionString("BookDb"));
    }
);

builder.Services.AddCors(io =>
{
    io.AddPolicy("PipelinePolicy",
        builder =>
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.WithOrigins("https://localhost:7075")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

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
app.UseCors("PipelinePolicy");
app.MapControllers();

app.Run();
