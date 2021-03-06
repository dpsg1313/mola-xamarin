﻿using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MolaApp.Api
{
    public interface IProfileApi : IGetObjectApi<ProfileModel>
    {
        Task<bool> UpdateAsync(ProfileModel profile);
    }
}
