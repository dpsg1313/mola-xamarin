using MolaApp.Model;
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
    class ProfileEditViewModel : INotifyPropertyChanged
    {
        private const string NO_STAGE = "nicht anzeigen";
        private const string NO_RELATIONSHIP_STATUS = "nicht anzeigen";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void WriteToModel(ProfileModel model)
        {
            model.DioceseId = selectedDiocese?.Id ?? "";
            model.RegionId = selectedRegion?.Id ?? "";
            model.TribeId = selectedTribe?.Id ?? "";
            model.FunctionId = selectedFunction?.Id ?? "";

            if(FavouriteStage == NO_STAGE)
            {
                model.FavouriteStage = "";
            }
            else
            {
                model.FavouriteStage = favouriteStage ?? "";
            }

            if (RelationshipStatus == NO_RELATIONSHIP_STATUS)
            {
                model.RelationshipStatus = "";
            }
            else
            {
                model.RelationshipStatus = relationshipStatus ?? "";
            }

            if (string.IsNullOrEmpty(georgesPoints))
            {
                model.GeorgesPoints = -1;
            }
            else
            {
                model.GeorgesPoints = int.Parse(georgesPoints);
            }

            model.Firstname = firstname;
            model.Lastname = lastname;
            model.Residence = residence;
            model.Phone = phone;
            model.Mail = mail;
            model.WoodbadgeCount = woodbadgeCount;
        }

        IList<Diocese> dioceseList;
        public IList<Diocese> DioceseList
        {
            get { return dioceseList; }
            set
            {
                if (dioceseList != value)
                {
                    dioceseList = value;
                    OnPropertyChanged(nameof(DioceseList));
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

                    if (selectedDiocese != null)
                    {
                        if (selectedDiocese.HasRegions)
                        {
                            // fill, enable and reset region picker
                            RegionList = selectedDiocese.Regions.Values.ToList();
                            IsRegionEnabled = true;
                            selectedRegion = null;

                            // reset and disable tribe picker
                            selectedTribe = null;
                            IsTribeEnabled = false;
                        }
                        else
                        {
                            // reset and disable region picker
                            RegionList = new List<Region>();
                            selectedRegion = null;
                            IsRegionEnabled = false;

                            // fill, enable and reset tribe picker
                            TribeList = selectedDiocese.Tribes.Values.ToList();
                            selectedTribe = null;
                            IsTribeEnabled = true;
                        }
                    }
                    else
                    {
                        // reset and disable region and tribe pickers
                        selectedRegion = null;
                        IsRegionEnabled = false;
                        selectedTribe = null;
                        IsTribeEnabled = false;
                    }

                    OnPropertyChanged();
                }
            }
        }

        bool isRegionEnabled;
        public bool IsRegionEnabled
        {
            get
            {
                return isRegionEnabled;
            }
            set
            {
                if (isRegionEnabled != value)
                {
                    isRegionEnabled = value;
                    OnPropertyChanged(nameof(IsRegionEnabled));
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
                    OnPropertyChanged(nameof(RegionList));
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

                    if (selectedRegion != null)
                    {
                        // fill, enable and reset tribe picker
                        TribeList = selectedRegion.Tribes.Values.ToList();
                        selectedTribe = null;
                        IsTribeEnabled = true;
                    }
                    else
                    {
                        // reset and disable tribe picker
                        selectedTribe = null;
                        IsTribeEnabled = false;
                    }

                    OnPropertyChanged();
                }
            }
        }

        bool isTribeEnabled;
        public bool IsTribeEnabled
        {
            get { return isTribeEnabled; }
            set
            {
                if (isTribeEnabled != value)
                {
                    isTribeEnabled = value;
                    OnPropertyChanged(nameof(IsTribeEnabled));
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
                    OnPropertyChanged(nameof(TribeList));
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
                    OnPropertyChanged(nameof(FunctionList));
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
                    OnPropertyChanged(nameof(SelectedFunction));
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

        string firstname;
        public string Firstname
        {
            get { return firstname; }
            set
            {
                if (firstname != value)
                {
                    firstname = value;
                    OnPropertyChanged(nameof(Firstname));
                }
            }
        }

        string lastname;
        public string Lastname
        {
            get { return lastname; }
            set
            {
                if (lastname != value)
                {
                    lastname = value;
                    OnPropertyChanged(nameof(Lastname));
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

        public List<string> StagesList
        {
            get {
                return new List<string>()
                {
                    NO_STAGE,
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
                    OnPropertyChanged(nameof(FavouriteStage));
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

        public List<string> RelationshipStatusList
        {
            get
            {
                return new List<string>()
                {
                    NO_RELATIONSHIP_STATUS,
                    "single",
                    "in einer festen Beziehung",
                    "in einer offenen Beziehung",
                    "verlobt",
                    "verheiratet",
                    "kompliziert"
                };
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
    }
}
