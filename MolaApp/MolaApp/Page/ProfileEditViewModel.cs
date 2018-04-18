using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MolaApp.Page
{
    class ProfileEditViewModel : INotifyPropertyChanged
    {
        IList<Diocese> dioceseList;
        public IList<Diocese> DioceseList
        {
            get { return dioceseList; }
            set
            {
                if (dioceseList != value)
                {
                    dioceseList = value;
                    OnPropertyChanged();
                }
            }
        }

        Diocese selectedDiocese;
        public Diocese SelectedDiocese
        {
            get { return selectedDiocese; }
            set
            {
                if (selectedDiocese != value)
                {
                    selectedDiocese = value;
                    OnPropertyChanged();
                }
            }
        }

        IList<Region> regionList;
        public IList<Region> RegionList
        {
            get { return regionList; }
            set
            {
                if (regionList != value)
                {
                    regionList = value;
                    OnPropertyChanged();
                }
            }
        }

        Region selectedRegion;
        public Region SelectedRegion
        {
            get { return selectedRegion; }
            set
            {
                if (selectedRegion != value)
                {
                    selectedRegion = value;
                    OnPropertyChanged();
                }
            }
        }

        IList<Tribe> tribeList;
        public IList<Tribe> TribeList
        {
            get { return tribeList; }
            set
            {
                if (tribeList != value)
                {
                    tribeList = value;
                    OnPropertyChanged();
                }
            }
        }

        Tribe selectedTribe;
        public Tribe SelectedTribe
        {
            get { return selectedTribe; }
            set
            {
                if (selectedTribe != value)
                {
                    selectedTribe = value;
                    OnPropertyChanged();
                }
            }
        }

        IList<Function> functionList;
        public IList<Function> FunctionList
        {
            get { return functionList; }
            set
            {
                if (functionList != value)
                {
                    functionList = value;
                    OnPropertyChanged();
                }
            }
        }

        Function selectedFunction;
        public Function SelectedFunction
        {
            get { return selectedFunction; }
            set
            {
                if (selectedFunction != value)
                {
                    selectedFunction = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public List<string> StagesList
        {
            get {
                return new List<string>()
                {
                    "Wölflinge",
                    "Jungpfadfinder",
                    "Pfadfinder",
                    "Rover"
                };
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
                    OnPropertyChanged();
                }
            }
        }

        public List<int> WoodbadgeList
        {
            get
            {
                return new List<int>()
                {
                    0,
                    2,
                    3,
                    4,
                    5,
                    6
                };
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
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void WriteToModel(ProfileModel model)
        {
            model.DioceseId = selectedDiocese.Id;
            model.RegionId = selectedRegion.Id;
            model.TribeId = selectedTribe.Id;

            model.Name = name;
            model.Residence = residence;
            model.Phone = phone;
            model.Mail = mail;
            model.FunctionId = selectedFunction.Id;
            model.WoodbadgeCount = woodbadgeCount;
        }
    }
}
