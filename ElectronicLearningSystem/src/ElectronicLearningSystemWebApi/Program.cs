using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var confuguration = builder.Configuration;

// Настройка Kestrel.
builder.WebHost.ConfigureKestrelServer(5000);

// Добавление Swagger.
builder.Services.AddSwagger();

// Настройка CORS.
builder.Services.AddCustomCors();

// Настройка базы данных.
builder.Services.AddDatabaseContext(builder.Configuration);

// Настройка SignalR.
builder.Services.AddCustomSignalR();

// Регистрация репозиториев и сервисов.
builder.Services.AddCustomServices();

// Настройка AutoMapper.
builder.Services.AddAutoMapperConfiguration();

// Регистрация Kafka Producer.
builder.Services.AddKafkaProducer(builder.Configuration);

// Регистрация Redis.
builder.Services.AddRedis(builder.Configuration);

// Настройка аутентификации и авторизации.
builder.Services.AddJwtAuthentication(builder.Configuration);

// Настройка контроллеров.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Инициализация базы данных.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    context.Database.EnsureCreated();
    DataBaseInitializer.Initialize(context);
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationhub");

app.Run();
