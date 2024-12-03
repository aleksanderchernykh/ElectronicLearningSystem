using ElectronicLearningSystemKafka.Common.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemKafka.Core.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging(config =>
    {
        config.ClearProviders();
        config.AddConsole();
        config.SetMinimumLevel(LogLevel.Information);
    })
    .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Consumer>>();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var kafkaBrokerUrl = configuration["Kafka:KafkaBrokerUrl"];
var schemaRegistryUrl = configuration["Kafka:SchemaRegistryUrl"];
using var consumer = new Consumer(logger, kafkaBrokerUrl, schemaRegistryUrl);
consumer.SubscribeTopic<string, Email>("emailreader", TopicEnum.EmailSending, topic => Console.WriteLine($"Заголовок {topic.Message.Value.Subject}"));