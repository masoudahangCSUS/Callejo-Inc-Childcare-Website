using System;

namespace Common.View
{
    public class SecretDTO
    {
        public Guid FkUser { get; set; }
        public string Secret { get; set; }
    }
}
