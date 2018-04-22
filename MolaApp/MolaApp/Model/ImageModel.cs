using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace MolaApp.Model
{
    public class ImageModel : IModel
    {
        public string Id { get; }

        public byte[] Bytes { get; set; }

        public ImageModel(string id)
        {
            Id = id;
        }
    }
}
