using System;
using System.Threading.Tasks;
using FirebaseCloudMessaging.Data;
using FirebaseCloudMessaging.Data.Models;
using FirebaseCloudMessaging.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirebaseCloudMessaging.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        public NotificationService(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task SaveTokenAsync(string tokenValue)
        {
            var isTokenValid = await IsTokenValidAsync(tokenValue);
            if (isTokenValid)
            {
                var newToken = new Token
                {
                    Value = tokenValue,
                    IsActive = true,
                    CreatedUtc = DateTime.UtcNow
                };

                await _dbContext.Tokens.AddAsync(newToken);
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task<bool> IsTokenValidAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            var tokenExist = await _dbContext.Tokens.AnyAsync(x => x.Value == value);
            return !tokenExist;
        }
    }
}
