using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FirebaseCloudMessaging.Data;
using FirebaseCloudMessaging.Data.Models;
using FirebaseCloudMessaging.Services.Interfaces;
using FirebaseCloudMessaging.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FirebaseCloudMessaging.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly IOptions<FcmConfig> _fcmConfig;
        private readonly string[] _badTokenRespMessange = { "NotRegistered", "InvalidRegistration", "MismatchSenderId" };
        public NotificationService(ApplicationDbContext context, HttpClient httpClient, IOptions<FcmConfig> fcmConfig)
        {
            _dbContext = context;
            _httpClient = httpClient;
            _fcmConfig = fcmConfig;
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

        public async Task<SendNotificationResponse> SendNotificationAsync(NotificationModel notification)
        {
            var sendResponse = new FcmSendResponse()
            {
                Success = 0
            };
            var tokens = await GetActiveTokensAsync();
            if (tokens?.Any() == true)
            {
                try
                {
                    string content = JsonConvert.SerializeObject(new
                    {
                        registration_ids = tokens,
                        priority = "high",
                        data = new
                        {
                            message = notification.Message,
                            title = notification.Title,
                            customProperty = "This is my custom property value"
                        },
                        notification = new
                        {
                            body = notification.Message,
                            title = notification.Title,
                            icon = notification.Icon,
                            click_action = notification.Link,
                            sound = "default",
                            content_available = true
                        }
                    });
                    var body = new StringContent(content, Encoding.UTF8, "application/json");
                    sendResponse = await ExecuteFcmPostAsync(body).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    //ignored
                }
            }

            return await ValidateResponseAsync(sendResponse, tokens.ToArray());
        }

        private async Task<bool> IsTokenValidAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            var tokenExist = await _dbContext.Tokens.AnyAsync(x => x.Value == value);
            return !tokenExist;
        }

        private async Task<IEnumerable<string>> GetActiveTokensAsync()
        {
            return await _dbContext.Tokens.Where(x => x.IsActive).Select(x => x.Value).ToListAsync();
        }

        private async Task<FcmSendResponse> ExecuteFcmPostAsync(StringContent body)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + _fcmConfig.Value.Key);
            var response = await _httpClient.PostAsync(_fcmConfig.Value.EndPoint, body).ConfigureAwait(false);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var sendResponse = JsonConvert.DeserializeObject<FcmSendResponse>(responseContent);
            return sendResponse;
        }

        private async Task<SendNotificationResponse> ValidateResponseAsync(FcmSendResponse sendStatus, string[] tokens)
        {
            SendNotificationResponse response;
            if (sendStatus.IsAllSuccess())
            {
                response = new SendNotificationResponse
                {
                    SendNotificationStatus = SendNotificationEnum.Success,
                    StatusMessage = "Notification has been successfully sent!",
                };
            }
            else if (sendStatus.IsAllFail())
            {
                response = new SendNotificationResponse
                {
                    SendNotificationStatus = SendNotificationEnum.NotificationPostFail,
                    StatusMessage = "Notification has not been sent!"
                };
            }
            else
            {
                response = new SendNotificationResponse
                {
                    SendNotificationStatus = SendNotificationEnum.NotAllSuccess,
                    StatusMessage = "Some notification has not been sent!"
                };
            }

            var tokensForMarkAsInactive = sendStatus.Results
                .Select((r, index) => new
                {
                    Result = r,
                    Index = index
                })
                .Where(r => _badTokenRespMessange.Contains(r.Result.Error))
                .Select(resultWithIndex => tokens[resultWithIndex.Index])
                .ToArray();

            await RemoveTokensAsync(tokensForMarkAsInactive);



            return response;
        }

        private async Task RemoveTokensAsync(string[] tokens)
        {
            if (tokens?.Any() == true)
            {
                var dbTokens = await _dbContext.Tokens
                    .Where(b => tokens.Contains(b.Value) && b.IsActive)
                    .ToListAsync();

                if (dbTokens.Count > 0)
                {
                    foreach (var token in dbTokens)
                    {
                        token.IsActive = false;
                        token.ModifiedUtc = DateTime.UtcNow;
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
