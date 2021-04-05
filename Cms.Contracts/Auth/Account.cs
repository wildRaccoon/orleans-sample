using System;
using System.Collections.Generic;

namespace Cms.Contracts.Auth
{
    public class Account
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public List<string> Groups { get; set; }

        public int FailedRetries { get; set; }

        public bool IsLocked { get; set; }

        public DateTime LastSuccessLogin { get; set; }
    }
}