using Avro.Generic;
using ElectronicLearningSystemKafka.Core.Producer;
using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers.Jwt;
using ElectronicLearningSystemWebApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var confuguration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(confuguration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddScoped<TokenHelper>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<Producer>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Producer>>();
    return new Producer(logger, confuguration.GetConnectionString("KafkaBrokerUrl"), confuguration.GetConnectionString("SchemaRegistryUrl"));
});

var key = Encoding.ASCII.GetBytes(confuguration["Jwt:Key"]);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // ������� ��� ��������-URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
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
        IssuerSigningKey = new SymmetricSecurityKey(key)
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
