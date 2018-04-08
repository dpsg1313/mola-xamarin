﻿using MolaApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    class ProfileApi : AbstractApi<ProfileModel>, IProfileApi
    {
        string baseUrl = "http://encala.de/";

        protected override string GetBaseUrl()
        {
            return baseUrl;
        }
    }
}