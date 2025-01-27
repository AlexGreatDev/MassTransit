using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using KeyGenerator.BackgroundServices;
using KeyGenerator.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateDefaultBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.UseSerilog();
builder.UseServiceProviderFactory(new DryIocServiceProviderFactory());
builder.ConfigureServices((hostContext, services) =>
{

    services.AddMassTransit(x =>
    {

        x.AddConsumer<SessionKeyRequestService>();
        x.AddConsumer<SessionKeyNotificationService>();

        x.UsingInMemory((context, cfg) =>
        {
            cfg.ReceiveEndpoint("in-memory-queue", ep =>
            {
                ep.ConfigureConsumer<SessionKeyRequestService>(context);
                ep.ConfigureConsumer<SessionKeyNotificationService>(context);
            });

        });
    });
    services.AddHostedService<MessagePublishingBackgroundService>();

});

var app = builder.Build();

await app.RunAsync();
