using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp
{
    public class AuthToken
    {
        public string UserId { get; }
        public string Token { get; }

        public AuthToken(string userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
