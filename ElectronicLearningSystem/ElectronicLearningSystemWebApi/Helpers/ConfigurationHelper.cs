using ElectronicLearningSystemKafka.Core.Producer;
using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers.Services;
using ElectronicLearningSystemWebApi.Helpers.Mapper;
using ElectronicLearningSystemWebApi.Repositories.Base;
using ElectronicLearningSystemWebApi.Repositories.Notification;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace ElectronicLearningSystemWebApi.Helpers
{
    /// <summary>
    /// Хелпер для конфигурации api.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Настройка конфигурации Swagger.
        /// </summary>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ElectronicLearningSystemWebApi"
                });

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
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Настройка конфигурации jwt аутентификации.
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
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
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]))
                };
            });

            return services;
        }

        /// <summary>
        /// Настройка конфигурации redis.
        /// </summary>
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]));

            return services;
        }

        /// <summary>
        /// Настройка конфигурации kafka.
        /// </summary>
        public static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<Producer>>();
                return new Producer(logger,
                    configuration.GetConnectionString("KafkaBrokerUrl"),
                    configuration.GetConnectionString("SchemaRegistryUrl"));
            });

            return services;
        }

        /// <summary>
        /// Настройка конфигурации пользовательских сервисов.
        /// </summary>
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<EmailSendingHelper>();
            services.AddScoped<UserService>();
            services.AddScoped<RedisHelper>();
            services.AddScoped<AuthService>();
            services.AddScoped<RoleService>();
            services.AddScoped<TaskService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<CommentService>();
            services.AddScoped<JwtTokenHelper>();

            return services;
        }

        /// <summary>
        /// Настройка конфигурации automapper.
        /// </summary>
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

        /// <summary>
        /// Настройка конфигурации signalr.
        /// </summary>
        public static IServiceCollection AddCustomSignalR(this IServiceCollection services)
        {
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            return services;
        }

        /// <summary>
        /// Настройка конфигурации DB.
        /// </summary>
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        /// <summary>
        /// Настройка конфигурации cors.
        /// </summary>
        public static IServiceCollection AddCustomCors(this IServiceCollection services, string[] allowedOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            return services;
        }

        /// <summary>
        /// Настройка конфигурации kestrel.
        /// </summary>
        public static IWebHostBuilder ConfigureKestrelServer(this IWebHostBuilder builder, int port)
        {
            builder.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(port);
            });

            return builder;
        }
    }
}
