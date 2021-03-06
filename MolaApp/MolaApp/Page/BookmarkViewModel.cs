﻿using MolaApp.Model;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MolaApp.Page
{
    public class BookmarkViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ProfileId { get; }

        public BookmarkViewModel(string profileId)
        {
            ProfileId = profileId;
            Image = ImageSource.FromResource("avatar.jpg");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ImageSource image;
        public ImageSource Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        string label;
        public string Label
        {
            get { return label; }
            set
            {
                if (label != value)
                {
                    label = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }
    }
}
