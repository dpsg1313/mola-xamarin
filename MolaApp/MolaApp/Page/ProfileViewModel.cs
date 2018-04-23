﻿using MolaApp.Model;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MolaApp.Page
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<AdventureViewModel> Adventures { get; }

        public ProfileViewModel()
        {
            Adventures = new ObservableCollection<AdventureViewModel>();
            showAdventureButton = true;
            Image = ImageSource.FromResource("avatar.jpg");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        bool isDataLoaded;
        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                if (isDataLoaded != value)
                {
                    isDataLoaded = value;
                    OnPropertyChanged(nameof(IsDataLoaded));
                }
            }
        }

        bool isMyProfile;
        public bool IsMyProfile
        {
            get { return isMyProfile; }
            set
            {
                if (isMyProfile != value)
                {
                    isMyProfile = value;
                    OnPropertyChanged(nameof(IsMyProfile));
                }
            }
        }

        bool isBookmarked;
        public bool IsBookmarked
        {
            get { return isBookmarked; }
            set
            {
                if (isBookmarked != value)
                {
                    isBookmarked = value;
                    OnPropertyChanged(nameof(IsBookmarked));
                }
            }
        }

        Diocese diocese;
        public Diocese Diocese
        {
            get { return diocese; }
            set
            {
                if (diocese != value)
                {
                    diocese = value;
                    OnPropertyChanged(nameof(Diocese));
                }
            }
        }

        Region region;
        public Region Region
        {
            get { return region; }
            set
            {
                if (region != value)
                {
                    region = value;
                    OnPropertyChanged(nameof(Region));
                }
            }
        }

        Tribe tribe;
        public Tribe Tribe
        {
            get { return tribe; }
            set
            {
                if (tribe != value)
                {
                    tribe = value;
                    OnPropertyChanged(nameof(Tribe));
                }
            }
        }

        Function function;
        public Function Function
        {
            get { return function; }
            set
            {
                if (function != value)
                {
                    function = value;
                    OnPropertyChanged(nameof(Function));
                }
            }
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

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        string mail;
        public string Mail
        {
            get { return mail; }
            set
            {
                if (mail != value)
                {
                    mail = value;
                    OnPropertyChanged(nameof(Mail));
                }
            }
        }

        string residence;
        public string Residence
        {
            get { return residence; }
            set
            {
                if (residence != value)
                {
                    residence = value;
                    OnPropertyChanged(nameof(Residence));
                }
            }
        }

        string favouriteStage;
        public string FavouriteStage
        {
            get { return favouriteStage; }
            set
            {
                if (favouriteStage != value)
                {
                    favouriteStage = value;
                    OnPropertyChanged(nameof(FavouriteStage));
                }
            }
        }

        int woodbadgeCount;
        public int WoodbadgeCount
        {
            get { return woodbadgeCount; }
            set
            {
                if (woodbadgeCount != value)
                {
                    woodbadgeCount = value;
                    OnPropertyChanged(nameof(WoodbadgeCount));
                }
            }
        }

        string georgesPoints;
        public string GeorgesPoints
        {
            get { return georgesPoints; }
            set
            {
                if (georgesPoints != value)
                {
                    georgesPoints = value;
                    OnPropertyChanged(nameof(GeorgesPoints));
                }
            }
        }

        string relationshipStatus;
        public string RelationshipStatus
        {
            get { return relationshipStatus; }
            set
            {
                if (relationshipStatus != value)
                {
                    relationshipStatus = value;
                    OnPropertyChanged(nameof(RelationshipStatus));
                }
            }
        }

        bool showAdventureButton;
        public bool ShowAdventureButton
        {
            get
            {
                return showAdventureButton && !isMyProfile;
            }
            set
            {
                if (showAdventureButton != value)
                {
                    showAdventureButton = value;
                    OnPropertyChanged(nameof(ShowAdventureButton));
                }
            }
        }
    }
}
