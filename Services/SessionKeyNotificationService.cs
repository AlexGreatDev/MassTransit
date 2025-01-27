using MassTransit;
using Microsoft.Extensions.Logging;
using KeyGenerator.Models;
using System.Security.Cryptography;
using System.Text;

namespace KeyGenerator.Services
{
    public class SessionKeyNotificationService : IConsumer<KeyNotification>
    {
        private readonly ILogger<SessionKeyNotificationService> _logger;

        public SessionKeyNotificationService(ILogger<SessionKeyNotificationService> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<KeyNotification> context)
        {
            var notification = context.Message;

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(notification.SessionKey.PadRight(32).Substring(0, 32));
            aes.GenerateIV();

            string aesKey = Convert.ToBase64String(aes.Key);
            string aesIV = Convert.ToBase64String(aes.IV);

            _logger.LogInformation($"AES Key: {aesKey}");
            _logger.LogInformation($"AES IV: {aesIV}");

            return Task.CompletedTask;
        }
    }
}


