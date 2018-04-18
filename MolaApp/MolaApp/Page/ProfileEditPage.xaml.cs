using MolaApp.Model;
using MolaApp.Repository;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileEditPage : MolaPage
	{
        ProfileModel model;

        ProfileRepository profileRepo;

        StructureController structureController;

        Dictionary<string, string> dioceseNameToId = new Dictionary<string, string>();

        public IList<Diocese> Dioceses;

        ProfileEditViewModel viewModel;

        public ProfileEditPage(ServiceContainer container, ProfileModel profile) : base(container)
        {
            profileRepo = Container.Get<ProfileRepository>("repository/profile");

            structureController = Container.Get<StructureController>("structure");

            InitializeComponent();

            model = profile;

            viewModel = new ProfileEditViewModel();
            BindingContext = viewModel;
            
            viewModel.DioceseList = structureController.Structure.Dioceses.Values.ToList();
            viewModel.FunctionList = structureController.Structure.Functions;
        }

        void SelectedDioceseChanged(object sender, EventArgs e)
        {
            if(viewModel.SelectedDiocese != null)
            {
                Diocese diocese = viewModel.SelectedDiocese;

                if (diocese.HasRegions)
                {
                    // Reset region and enable and fill picker
                    viewModel.RegionList = diocese.Regions.Values.ToList();
                    viewModel.SelectedRegion = null;
                    regionPicker.IsEnabled = true;

                    // reset tribe and disable picker
                    viewModel.SelectedTribe = null;
                    tribePicker.IsEnabled = false;
                }
                else
                {
                    // Reset region and disable picker
                    viewModel.RegionList = new List<Region>();
                    viewModel.SelectedRegion = null;
                    regionPicker.IsEnabled = false;

                    // reset tribe and enable and fill picker
                    viewModel.TribeList = diocese.Tribes.Values.ToList();
                    viewModel.SelectedTribe = null;
                    tribePicker.IsEnabled = true;
                }
            }
            else
            {
                viewModel.SelectedRegion = null;
                regionPicker.IsEnabled = false;
                viewModel.SelectedTribe = null;
                tribePicker.IsEnabled = false;
            }
        }

        void SelectedRegionChanged(object sender, EventArgs e)
        {
            if (viewModel.SelectedRegion != null)
            {
                Region region = viewModel.SelectedRegion;

                // reset tribe and enable and fill picker
                viewModel.TribeList = region.Tribes.Values.ToList();
                viewModel.SelectedTribe = null;
                tribePicker.IsEnabled = true;
            }
            else
            {
                viewModel.SelectedTribe = null;
                tribePicker.IsEnabled = false;
            }
        }

        async void SaveAsync(object sender, EventArgs e)
        {
            viewModel.WriteToModel(model);
            profileRepo.PutAsync(model);
            await Navigation.PopAsync();
        }

        async void CancelAsync(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void PictureOptionsAsync(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Profilbild", "Abbrechen", null, "Aufnehmen", "Auswählen", "Löschen");
            switch (action)
            {
                case "Aufnehmen":
                    TakePhotoAsync();
                    break;
                case "Auswählen":
                    PickPhotoAsync();
                    break;
                case "Löschen":
                    RemovePhotoAsync();
                    break;
                case "Abbrechen":
                default:
                    return;
            }
        }


        async void TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Fehler", "Dein Gerät unterstützt diese Funktion nicht", "OK");
                return;
            }

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Fehlende Berechtigung", "Die App darf keine Fotos aufnehmen", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() {
                AllowCropping = true,
                DefaultCamera = CameraDevice.Rear,
                MaxWidthHeight = 300,
                PhotoSize = PhotoSize.Small,
                Directory = "profile",
                Name = model.Id + ".jpg"
            });

            if (file == null)
            {
                return;
            }

            picture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        async void PickPhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Fehler", "Dein Gerät unterstützt diese Funktion nicht", "OK");
                return;
            }
            
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }

            if (storageStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Fehlende Berechtigung", "Die App darf keine Fotos aus der Galerie laden", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
            {
                return;
            }

            picture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        async void RemovePhotoAsync()
        {

        }
    }
}