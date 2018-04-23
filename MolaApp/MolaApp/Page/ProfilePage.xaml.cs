using MolaApp.Api;
using MolaApp.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : MolaPage
    {
        ProfileViewModel viewModel;

        StructureController structureController;
        BookmarkController bookmarkController;
        AuthController authController;
        ImageApi imageApi;
        ProfileApi profileApi;
        AdventureApi adventureApi;

        ProfileModel profile;

		public ProfilePage (ServiceContainer container, string profileId) : base(container)
		{
            structureController = Container.Get<StructureController>("structure");
            bookmarkController = Container.Get<BookmarkController>("bookmark");
            authController = Container.Get<AuthController>("auth");
            profileApi = Container.Get<ProfileApi>("api/profile");
            imageApi = Container.Get<ImageApi>("api/image");
            adventureApi = Container.Get<AdventureApi>("api/adventure");

            viewModel = new ProfileViewModel();
            viewModel.IsDataLoaded = false;

            InitializeComponent();
            BindingContext = viewModel;

            Task.Run(async () => {
                ProfileModel profile = await profileApi.GetAsync(profileId);
                SetModel(profile);
            });
        }

        private void SetModel(ProfileModel model)
        {
            profile = model;

            if(profile == null)
            {
                return;
            }

            viewModel.IsDataLoaded = true;

            viewModel.IsMyProfile = (model.Id == authController.AuthToken.UserId);

            viewModel.Name = model.Name;
            viewModel.Residence = model.Residence;
            viewModel.Phone = model.Phone;
            viewModel.Mail = model.Mail;
            viewModel.WoodbadgeCount = model.WoodbadgeCount;
            viewModel.FavouriteStage = model.FavouriteStage;
            viewModel.RelationshipStatus = model.RelationshipStatus;

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

            viewModel.IsBookmarked = bookmarkController.IsBookmarked(model.Id);

            Task.Run(async () => {
                if(String.IsNullOrEmpty(model.ImageId))
                {
                    viewModel.Image = ImageSource.FromResource("avatar.jpg");
                }
                else
                {
                    ImageModel image = await imageApi.GetAsync(model.ImageId);
                    viewModel.Image = ImageSource.FromStream(() => new MemoryStream(image.Bytes));
                }
            });

            adventureApi.Get(profile.Id).Subscribe(adventures =>
            {
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
                        viewModel.Adventures.Add(adventureViewModel);

                        profileApi.Get(adventure.WithUserId).Subscribe(withProfile =>
                        {
                            adventureViewModel.Label = withProfile.Name;
                        });
                    }
                }
            });
        }

        async void BookmarkToggled(object sender, EventArgs e)
        {
            if(profile  == null)
            {
                return;
            }

            if (viewModel.IsBookmarked)
            {
                await bookmarkController.AddBookmark(profile.Id);
            }
            else
            {
                await bookmarkController.RemoveBookmark(profile.Id);
            }
        }

        async void AdventureAsync(object sender, EventArgs e)
        {
            if (profile == null)
            {
                return;
            }

            bool doIt = await DisplayAlert("Abenteuer eintragen", "Wenn du ein Abenteuer einträgst, muss es zunächst von " + profile.Firstname + " bestätigt werden. Anschließend wird es in euren Profilen angezeigt. Bist du sicher, dass du das Abenteuer eintragen möchtest?", "Ja", "Nein");

            if (!doIt)
            {
                return;
            }

            ProfileModel myProfile = await profileApi.GetAsync(authController.AuthToken.UserId);

            int myPoints = await CalculatePointsAsync(myProfile, profile);
            int otherPoints = await CalculatePointsAsync(profile, myProfile);

            bool success = await adventureApi.AddAsync(profile.Id, myPoints, otherPoints);

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

        async Task<int> CalculatePointsAsync(ProfileModel thisProfile, ProfileModel otherProfile)
        {
            return 0;
        }
    }
}