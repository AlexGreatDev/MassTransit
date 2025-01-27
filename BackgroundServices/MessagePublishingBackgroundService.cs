using KeyGenerator.Models;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace KeyGenerator.BackgroundServices
{


    public class MessagePublishingBackgroundService : BackgroundService
    {
        private readonly IBus _bus;
        private readonly ILogger<MessagePublishingBackgroundService> _logger;

        public MessagePublishingBackgroundService(IBus bus, ILogger<MessagePublishingBackgroundService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MessagePublishingBackgroundService started.");

            var request = new SessionKeyRequest
            {
                InputString = "test_for_Jason ",
                KeyLength = 16
            };

            _logger.LogInformation("Publishing SessionKeyRequest: InputString={InputString}, KeyLength={KeyLength}",
                request.InputString, request.KeyLength);

            await _bus.Publish(request, stoppingToken);

            _logger.LogInformation("SessionKeyRequest published successfully.");
        }
    }

}
