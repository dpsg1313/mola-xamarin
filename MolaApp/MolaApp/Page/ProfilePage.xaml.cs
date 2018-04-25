using MolaApp.Api;
using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : MolaPage
    {
        const int MAX_ADVENTURES = 5;

        ProfileViewModel viewModel;

        StructureController structureController;
        BookmarkController bookmarkController;
        AuthController authController;
        ImageApi imageApi;
        ProfileApi profileApi;
        AdventureApi adventureApi;

        string profileId;
        ProfileModel profile;

        ProfileModel myProfile;

        CancellationTokenSource ctsMain;
        CancellationTokenSource ctsSecondary;
        CancellationTokenSource ctsAdventureProfiles;

        public ProfilePage (ServiceContainer container, string profileId) : base(container)
		{
            structureController = Container.Get<StructureController>("structure");
            bookmarkController = Container.Get<BookmarkController>("bookmark");
            authController = Container.Get<AuthController>("auth");
            profileApi = Container.Get<ProfileApi>("api/profile");
            imageApi = Container.Get<ImageApi>("api/image");
            adventureApi = Container.Get<AdventureApi>("api/adventure");

            this.profileId = profileId;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ctsMain = new CancellationTokenSource();

            viewModel = new ProfileViewModel();
            viewModel.IsDataLoaded = false;
            BindingContext = viewModel;

            profileApi.Get(profileId).Subscribe(SetModel, ctsMain.Token);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ctsMain.Cancel();

            if(ctsSecondary != null)
            {
                ctsSecondary.Cancel();
            }

            if(ctsAdventureProfiles != null)
            {
                ctsAdventureProfiles.Cancel();
            }
        }

        private void SetModel(ProfileModel model)
        {
            profile = model;

            if (profile == null)
            {
                return;
            }

            if(ctsSecondary != null)
            {
                ctsSecondary.Cancel();
            }
            ctsSecondary = new CancellationTokenSource();

            viewModel.IsDataLoaded = true;

            viewModel.IsMyProfile = (model.Id == authController.AuthToken.UserId);

            viewModel.IsBookmarked = bookmarkController.IsBookmarked(model.Id);

            viewModel.Name = model.Name;
            viewModel.Firstname = model.Firstname;
            viewModel.Lastname = model.Lastname;
            viewModel.Residence = model.Residence;
            viewModel.Phone = model.Phone;
            viewModel.Mail = model.Mail;
            viewModel.WoodbadgeCount = model.WoodbadgeCount;
            viewModel.FavouriteStage = model.FavouriteStage;
            viewModel.RelationshipStatus = model.RelationshipStatus;

            viewModel.ShowContact = !(string.IsNullOrEmpty(viewModel.Phone) && string.IsNullOrEmpty(viewModel.Mail));

            if (model.GeorgesPoints < 0)
            {
                viewModel.GeorgesPoints = "";
            }
            else
            {
                viewModel.GeorgesPoints = model.GeorgesPoints.ToString();
            }

            if (!String.IsNullOrEmpty(model.DioceseId))
            {
                Diocese diocese = structureController.Structure.Dioceses[model.DioceseId];
                viewModel.Diocese = diocese;
                if (diocese.HasRegions)
                {
                    if (!String.IsNullOrEmpty(model.RegionId))
                    {
                        Region region = diocese?.Regions[model.RegionId];
                        viewModel.Region = region;
                        if (!String.IsNullOrEmpty(model.TribeId))
                        {
                            Tribe tribe = region?.Tribes[model.TribeId];
                            viewModel.Tribe = tribe;
                        }
                    }
                }
                else
                {
                    viewModel.Region = null;
                    if (!String.IsNullOrEmpty(model.TribeId))
                    {
                        Tribe tribe = diocese?.Tribes[model.TribeId];
                        viewModel.Tribe = tribe;
                    }
                }
            }

            if (!String.IsNullOrEmpty(model.FunctionId))
            {
                viewModel.Function = structureController.Structure.Functions[model.FunctionId];
            }

            if(!String.IsNullOrEmpty(model.ImageId))
            {
                imageApi.Get(model.ImageId).Subscribe(image =>
                {
                    viewModel.Image = ImageSource.FromStream(() => new MemoryStream(image.Bytes));
                }, ctsSecondary.Token);
            }

            adventureApi.Get(profile.Id).Subscribe(adventures =>
            {
                if(ctsAdventureProfiles != null)
                {
                    ctsAdventureProfiles.Cancel();
                }
                ctsAdventureProfiles = new CancellationTokenSource();

                viewModel.ShowAdventures = true;
                viewModel.Adventures.Clear();
                float adventurePoints = 0;
                foreach(AdventureModel adventure in adventures)
                {
                    if (adventure.WithUserId == authController.AuthToken.UserId && adventure.WithConfirmed)
                    {
                        viewModel.ShowAdventureButton = false;
                    }

                    if (adventure.Confirmed && adventure.WithConfirmed)
                    {
                        // adventure confirmed from both users => show on profile page
                        AdventureViewModel adventureViewModel = new AdventureViewModel();
                        adventureViewModel.Label = adventure.WithUserId;
                        adventureViewModel.Points = adventure.Points.ToString();
                        viewModel.Adventures.Add(adventureViewModel);
                        adventurePoints += adventure.Points;

                        profileApi.Get(adventure.WithUserId).Subscribe(withProfile =>
                        {
                            adventureViewModel.Label = withProfile.Name;
                            if (!string.IsNullOrEmpty(withProfile.ImageId))
                            {
                                imageApi.Get(withProfile.ImageId).Subscribe(image =>
                                {
                                    adventureViewModel.Image = ImageSource.FromStream(() => new MemoryStream(image.Bytes));
                                }, ctsAdventureProfiles.Token);
                            }
                        }, ctsAdventureProfiles.Token);
                    }
                }

                int adventureCount = viewModel.Adventures.Count;
                FormattedString pointsText = new FormattedString();
                pointsText.Spans.Add(new Span { Text = profile.Firstname, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
                pointsText.Spans.Add(new Span { Text = " hat insgesamt ", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
                pointsText.Spans.Add(new Span { Text = adventurePoints.ToString(), FontSize = 60, FontAttributes = FontAttributes.Bold, TextColor = Color.SteelBlue });
                pointsText.Spans.Add(new Span { Text = " Punkte bei " + adventureCount.ToString() + " Abenteuer" + (adventureCount > 1 ? "n" : "") + " gesammelt", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
                viewModel.AdventurePoints = pointsText;

                if(viewModel.Adventures.Count > 0)
                {
                    viewModel.ShowAdventures = true;

                    // resize the listview to match content
                    adventuresList.HeightRequest = adventuresList.RowHeight * Math.Min(viewModel.Adventures.Count, MAX_ADVENTURES);
                }

            }, ctsSecondary.Token);

            profileApi.Get(authController.AuthToken.UserId).Subscribe(async myProfile =>
            {
                this.myProfile = myProfile;
                float possiblePoints = await CalculatePointsAsync(myProfile, profile);

                FormattedString hint = new FormattedString();
                hint.Spans.Add(new Span { Text = possiblePoints.ToString(), FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), FontAttributes = FontAttributes.Bold });
                hint.Spans.Add(new Span { Text = " Punkte bekommst du für ein Abenteuer mit " });
                hint.Spans.Add(new Span { Text = model.Firstname });
                viewModel.PossiblePoints = hint;
            }, ctsSecondary.Token);
        }
        
        async void BookmarkToggled(object sender, EventArgs e)
        {
            if(profile  == null)
            {
                return;
            }

            if (viewModel.IsBookmarked)
            {
                await bookmarkController.RemoveBookmark(profile.Id);
                viewModel.IsBookmarked = false;
                DependencyService.Get<IToastMessage>().ShortAlert("Profil von der Merkliste entfernt");
            }
            else
            {
                await bookmarkController.AddBookmark(profile.Id);
                viewModel.IsBookmarked = true;
                DependencyService.Get<IToastMessage>().ShortAlert("Profil gemerkt");
            }
        }

        async void AdventureAsync(object sender, EventArgs e)
        {
            if (profile == null || myProfile == null)
            {
                return;
            }

            bool doIt = await DisplayAlert("Abenteuer eintragen", "Damit das Abenteuer in euren Profilen angezeigt wird, muss " + profile.Firstname + " auch ein Abenteuer mit dir eintragen, denn nur dann gilt das Abenteuer als bestätigt. Bist du sicher, dass du das Abenteuer eintragen möchtest?", "Ja", "Nein");

            if (!doIt)
            {
                return;
            }

            float myPoints = await CalculatePointsAsync(myProfile, profile);
            float withPoints = await CalculatePointsAsync(profile, myProfile);

            bool success = await adventureApi.AddAsync(profile.Id, myPoints, withPoints);

            if (!success)
            {
                DependencyService.Get<IToastMessage>().LongAlert("Beim Eintragen des Abenteuers ist ein Fehler aufgetreten. Möglicherweise wurde es bereits zuvor eingetragen oder du hast gerade keine ausreichende Internetverbindung.");
            }
            else
            {
                DependencyService.Get<IToastMessage>().ShortAlert("Abenteuer wurde eingetragen.");
            }
        }

        void OnAdventureSelected(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        void ShowAdventureInfo(object sender, ItemTappedEventArgs e)
        {
            DisplayAlert(
                "Was ist ein Abenteuer?", 
                "Während es bei den Georgspunkten eine eher konkrete Vorgabe gibt, wofür man Punkte erhält, wollen wir - dem pfadfinderischen Gedanken folgend - jede wertvolle Begegnung zwischen zwei Menschen gleichermaßen würdigen. " + Environment.NewLine
                + Environment.NewLine
                + "Deshalb überlassen wir dir die Interpretation der Abenteuerpunkte. " + Environment.NewLine
                + Environment.NewLine
                + "Was bedeutet ‚Abenteuer‘ für dich?", 
                "Ok");
        }

        async Task<float> CalculatePointsAsync(ProfileModel getProfile, ProfileModel giveProfile)
        {
            return await Task.Run(() => CalculatePoints(getProfile, giveProfile));
        }

        float CalculatePoints(ProfileModel getProfile, ProfileModel giveProfile)
        {
            Function giveFunction;
            if(!structureController.Structure.Functions.TryGetValue(giveProfile.FunctionId, out giveFunction))
            {
                return 0;
            }

            string sameGroupLevel = SameGroupLevel(getProfile, giveProfile);
            if(sameGroupLevel == null)
            {
                return 0;
            }

            float points = DistanceCorrection(FunctionBasePoints(giveFunction.Id), giveFunction.Level, sameGroupLevel);

            points += AssociationBonus(giveProfile);

            points += giveProfile.WoodbadgeCount * 0.5f; // 1/2 point per woodbadge bead

            if (giveProfile.IsPriest)
            {
                // If person is a priest then points are doubled
                points *= 2;
            }

            return points;
        }

        float DistanceCorrection(float functionBasePoints, Function.Levels functionLevel, string sameGroupLevel)
        {
            float points = functionBasePoints;
            switch (functionLevel)
            {
                case Function.Levels.None:
                case Function.Levels.Bund:
                    break;

                case Function.Levels.Diocese:
                    if(sameGroupLevel == "world")
                    {
                        points += 2;
                    }
                    break;

                case Function.Levels.Region:
                    if (sameGroupLevel == "world")
                    {
                        points += 2;
                    }
                    else if(sameGroupLevel == "diocese")
                    {
                        points += 1;
                    }
                    break;

                case Function.Levels.Tribe:
                    if (sameGroupLevel == "world")
                    {
                        points += 2;
                    }
                    else if (sameGroupLevel == "diocese")
                    {
                        points += 1;
                    }
                    else if(sameGroupLevel == "tribe")
                    {
                        // functions on tribe level don't give points for members of the same tribe
                        points = 0;
                    }
                    break;
            }
            return points;
        }

        float FunctionBasePoints(string functionId)
        {
            switch (functionId)
            {
                case "0-1": // kein Amt
                    return 0;
                case "1-1": // Leiter
                    return 1;
                case "1-2": // StaVo
                    return 5;
                case "2-1": // BezirksAK
                    return 6;
                case "2-2": // Bezirksreferent
                    return 10;
                case "2-3": // BeVo
                    return 15;
                case "3-1": // DiöAK
                    return 14;
                case "3-2": // Diöreferent
                    return 20;
                case "3-3": // DeVo
                    return 30;
                case "4-1": // BundesAK
                    return 35;
                case "4-2": // Bundeshauptamt
                    return 35;
                case "4-3": // Bundesreferent
                    return 40;
                case "4-4": // BuVo
                    return 50;
                case "5-1": // International
                    return 48;
            }
            return 0;
        }

        string SameGroupLevel(ProfileModel profileA, ProfileModel profileB)
        {
            String sameGroupLevel = "world";

            if (profileB.DioceseId == profileA.DioceseId)
            {
                sameGroupLevel = "diocese";

                Diocese diocese;
                if (!structureController.Structure.Dioceses.TryGetValue(profileB.DioceseId, out diocese))
                {
                    return null;
                }

                if (profileB.TribeId == profileA.TribeId)
                {
                    sameGroupLevel = "tribe";
                }
                else
                {
                    if (diocese.HasRegions && profileB.RegionId == profileA.RegionId)
                    {
                        sameGroupLevel = "region";
                    }
                }
            }
            return sameGroupLevel;
        }

        float AssociationBonus(ProfileModel profile)
        {
            float bonus = 0;
            switch (profile.Association)
            {
                case "kein e.V.-Mitglied":
                    bonus = 0;
                    break;
                case "Stammes-e.V.":
                    bonus = 1;
                    break;
                case "Bezirks-e.V.":
                    bonus = 2;
                    break;
                case "Diözesan-e.V.":
                    bonus = 3;
                    break;
                case "Bundes-e.V.":
                    bonus = 5;
                    break;
            }
            if(profile.FunctionId == "0-1")
            {
                // if no function then bonus is doubled
                bonus *= 2;
            }
            return bonus;
        }
    }
}