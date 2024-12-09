using EmailSendingService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<KafkaConsumerWorker>();
        services.AddSingleton(hostContext.Configuration);
    })
    .RunConsoleAsync();