using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Api
{
    class ConflictException : Exception
    {
        public string Field
        {
            get;
        }

        public ConflictException(string field)
        {
            Field = field;
        }
    }
}
