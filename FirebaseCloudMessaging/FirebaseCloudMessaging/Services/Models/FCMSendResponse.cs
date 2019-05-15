using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FirebaseCloudMessaging.Services.Models
{
    public class FcmSendResponse
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("failure")]
        public int Failure { get; set; }

        [JsonProperty("results")]
        public MessageResult[] Results { get; set; }

        public bool IsAllSuccess()
        {
            int successResponseCount = Results?.Count(r => !String.IsNullOrWhiteSpace(r.MessageId)) ?? 0;
            int sentMessagesCount = Results?.Length ?? 0;
            return sentMessagesCount == successResponseCount;
        }

        public bool IsAllFail()
        {
            int successResponseCount = Results?.Count(r => String.IsNullOrWhiteSpace(r.MessageId)) ?? 0;
            int sentMessagesCount = Results?.Length ?? 0;
            return sentMessagesCount == successResponseCount;
        }
    }

    public class MessageResult
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
