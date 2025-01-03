﻿using ElectronicLearningSystemKafka.Core.Consumer;
using EmailSendingService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<KafkaConsumerWorker>();
        services.AddSingleton(hostContext.Configuration);
        services.AddTransient<EmailSender>();
        services.AddSingleton(sp => new Consumer(
            sp.GetRequiredService<ILogger<Consumer>>(),
            sp.GetRequiredService<IConfiguration>()["Kafka:KafkaBrokerUrl"],
            sp.GetRequiredService<IConfiguration>()["Kafka:SchemaRegistryUrl"]
        ));
    })
    .RunConsoleAsync();