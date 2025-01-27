using MassTransit;
using Microsoft.Extensions.Logging;
using KeyGenerator.Models;
using System.Security.Cryptography;

namespace KeyGenerator.Services
{

    public class SessionKeyRequestService : IConsumer<SessionKeyRequest>
    {
        private readonly ILogger<SessionKeyRequestService> _logger;

        public SessionKeyRequestService(ILogger<SessionKeyRequestService> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SessionKeyRequest> context)
        {
            var request = context.Message;

            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using var rfc = new Rfc2898DeriveBytes(
                password: request.InputString,
                salt: salt,
                iterations: 10000,
                hashAlgorithm: HashAlgorithmName.SHA256
            );
            string sessionKey = Convert.ToBase64String(rfc.GetBytes(request.KeyLength));

            _logger.LogInformation($"Session Key generated: {sessionKey}");
            _logger.LogInformation($"Salt: {Convert.ToBase64String(salt)}");

            await context.Publish(new KeyNotification
            {
                SessionKey = sessionKey
            });
        }
    }
}


