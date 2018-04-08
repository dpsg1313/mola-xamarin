using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp
{
    public class AuthToken
    {
        public string Token { get; }
        public DateTimeOffset Expires { get; }

        public AuthToken(string token, DateTimeOffset expires)
        {
            Token = token;
            Expires = expires;
        }
    }
}
