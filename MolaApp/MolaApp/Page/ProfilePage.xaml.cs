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
        ImageApi imageApi;
        ProfileApi profileApi;

        ProfileModel profile;

		public ProfilePage (ServiceContainer container, string profileId) : base(container)
		{
            structureController = Container.Get<StructureController>("structure");
            bookmarkController = Container.Get<BookmarkController>("bookmark");
            profileApi = Container.Get<ProfileApi>("api/profile");
            imageApi = Container.Get<ImageApi>("api/image");

            viewModel = new ProfileViewModel();

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

            viewModel.Bookmarked = bookmarkController.IsBookmarked(model.Id);

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
        }

        async void BookmarkToggled(object sender, EventArgs e)
        {
            if(profile  == null)
            {
                return;
            }

            if (viewModel.Bookmarked)
            {
                await bookmarkController.AddBookmark(profile.Id);
            }
            else
            {
                await bookmarkController.RemoveBookmark(profile.Id);
            }
        }

    }
}