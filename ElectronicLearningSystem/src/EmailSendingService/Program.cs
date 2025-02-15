using ElectronicLearningSystemKafka.Core.Consumer;
using EmailSendingService;
using EmailSendingService.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath("/app");
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        // Устанавливаем параметры почты из переменных окружения
        services.Configure<EmailSettings>(options =>
        {
            options.SmtpServerUrl = Environment.GetEnvironmentVariable("SMTP_SERVER");
            options.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
            options.SenderEmailAddress = Environment.GetEnvironmentVariable("EMAIL_SENDER");
            options.SenderPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        });

        // Регистрируем все зависимости
        services.AddHostedService<KafkaConsumerWorker>();

        // Регистрируем EmailSender, используя IOptions для конфигурации
        services.AddTransient<EmailSender>();

        // Регистрация Kafka Consumer с параметрами из конфигурации
        services.AddSingleton(sp => new Consumer(
            sp.GetRequiredService<ILogger<Consumer>>(),
            sp.GetRequiredService<IConfiguration>()["Kafka:KafkaBrokerUrl"],
            sp.GetRequiredService<IConfiguration>()["Kafka:SchemaRegistryUrl"]
        ));
    })
    .RunConsoleAsync();