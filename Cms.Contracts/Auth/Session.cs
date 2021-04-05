using Cms.Core.Rights;
using System;

namespace Cms.Contracts.Auth
{
    public class Session
    {
        public string Token { get; set; }
        public string AccountId { get; set; }
        public UserRights Rights { get; set; }
        public DateTime LastAccess { get; set; }
        public bool Expired { get; set; }
    }
}
