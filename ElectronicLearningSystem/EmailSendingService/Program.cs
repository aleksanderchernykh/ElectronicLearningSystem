using ElectronicLearningSystemKafka.Common.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemKafka.Core.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Настройка Dependency Injection для логирования
var serviceProvider = new ServiceCollection()
    .AddLogging(config =>
    {
        config.ClearProviders();
        config.AddConsole();
        config.SetMinimumLevel(LogLevel.Information);
    })
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Consumer>>();

// Настройка конфигурации
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var kafkaBrokerUrl = configuration["Kafka:KafkaBrokerUrl"];
var schemaRegistryUrl = configuration["Kafka:SchemaRegistryUrl"];

using var consumer = new Consumer(logger, kafkaBrokerUrl, schemaRegistryUrl);
var consumerTask = Task.Run(() =>
{
    consumer.SubscribeTopic<string, Email>(
        "emailreader",
        TopicEnum.EmailSending,
        topic => Console.WriteLine($"Заголовок {topic.Message.Value.Subject}")
    );
});

Console.ReadLine();
await consumerTask;

Console.WriteLine("Обработка завершена.");