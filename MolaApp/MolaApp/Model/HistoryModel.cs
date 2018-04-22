using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class HistoryModel
    {
        public string ProfileId { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}
