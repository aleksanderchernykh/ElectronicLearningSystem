using ElectronicLearningSystemKafka.Core.Producer;
using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Repositories.Base;
using ElectronicLearningSystemWebApi.Repositories.Notification;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var confuguration = builder.Configuration;

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "ElectronicLearningSystemWebApi"
    });

    // Настройка аутентификации
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {your_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(confuguration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<EmailSendingHelper>();
builder.Services.AddScoped<UserHelper>();
builder.Services.AddScoped<NotificationHelper>();
builder.Services.AddScoped<CommentHelper>();
builder.Services.AddScoped<JwtTokenHelper>();
builder.Services.AddSingleton(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Producer>>();
    return new Producer(logger, 
        confuguration.GetConnectionString("KafkaBrokerUrl"), 
        confuguration.GetConnectionString("SchemaRegistryUrl"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = confuguration["Jwt:Issuer"],
        ValidAudience = confuguration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(confuguration["Jwt:Key"]))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    context.Database.EnsureCreated();
    DataBaseInitializer.Initialize(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationhub");

app.Run();
