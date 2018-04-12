using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp
{
    public class AuthToken
    {
        public string UserId { get; }
        public string Token { get; }
        public DateTimeOffset Expires { get; }

        public AuthToken(string userId, string token, DateTimeOffset expires)
        {
            UserId = userId;
            Token = token;
            Expires = expires;
        }
    }
}
