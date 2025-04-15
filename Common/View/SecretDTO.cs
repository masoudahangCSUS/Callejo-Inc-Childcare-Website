using System;
using System.Text.Json.Serialization;

namespace Common.View
{
    public class SecretDTO
    {
        [JsonPropertyName("fkUser")]
        public Guid FkUser { get; set; }

        [JsonPropertyName("secret")]
        public string Secret { get; set; }
    }

}
