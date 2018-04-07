using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class UserModel : IModel
    {
        public string Id { get; }

        public string Email { get; set; }
        public string Password { get; set; }

        public UserModel(string id)
        {
            Id = id;
        }
    }
}
