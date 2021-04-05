using System;

namespace Cms.Contracts.Rights
{
    public class Session
    {
        public string Token { get; set; }

        public string AccountId { get; set; }

        public DateTime LastAccess { get; set; }
    }
}
