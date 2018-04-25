using MolaApp.Api;
using MolaApp.Model;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileEditPage : MolaPage
	{
        ProfileModel model;

        StructureController structureController;
        IProfileApi profileApi;
        ImageApi imageApi;

        ProfileEditViewModel viewModel;

        byte[] newImage;

        bool removeImage;

        CancellationTokenSource cts;

        public ProfileEditPage(ServiceContainer container, string profileId) : base(container)
        {
            structureController = Container.Get<StructureController>("structure");
            profileApi = Container.Get<IProfileApi>("api/profile");
            imageApi = Container.Get<ImageApi>("api/image");

            InitializeComponent();

            viewModel = new ProfileEditViewModel();
            BindingContext = viewModel;

            cts = new CancellationTokenSource();
            profileApi.Get(profileId).Subscribe(SetModel, cts.Token);
        }

        ~ProfileEditPage()  // destructor
        {
            cts.Cancel();
        }

        void SetModel(ProfileModel profile)
        {
            model = profile;

            viewModel.DioceseList = structureController.Structure?.Dioceses?.Values.ToList();
            viewModel.FunctionList = structureController.Structure?.Functions?.Values.ToList();

            if(profile == null)
            {
                return;
            }

            viewModel.Firstname = profile.Firstname;
            viewModel.Lastname = profile.Lastname;
            viewModel.Residence = profile.Residence;
            viewModel.Phone = profile.Phone;
            viewModel.Mail = profile.Mail;
            viewModel.WoodbadgeCount = profile.WoodbadgeCount;
            viewModel.FavouriteStage = profile.FavouriteStage;
            viewModel.RelationshipStatus = profile.RelationshipStatus;
            viewModel.Association = profile.Association;
            viewModel.IsPriest = profile.IsPriest;

            if (profile.GeorgesPoints < 0)
            {
                viewModel.GeorgesPoints = "";
            }
            else
            {
                viewModel.GeorgesPoints = profile.GeorgesPoints.ToString();
            }
                

            if (!String.IsNullOrEmpty(profile.DioceseId))
            {
                Diocese diocese = structureController.Structure.Dioceses[profile.DioceseId];
                viewModel.SelectedDiocese = diocese;
                if (diocese.HasRegions)
                {
                    if (!String.IsNullOrEmpty(profile.RegionId))
                    {
                        Region region = diocese?.Regions[profile.RegionId];
                        viewModel.SelectedRegion = region;
                        if (!String.IsNullOrEmpty(profile.TribeId))
                        {
                            Tribe tribe = region?.Tribes[profile.TribeId];
                            viewModel.SelectedTribe = tribe;
                        }
                    }
                }
                else
                {
                    viewModel.SelectedRegion = null;
                    if (!String.IsNullOrEmpty(profile.TribeId))
                    {
                        Tribe tribe = diocese?.Tribes[profile.TribeId];
                        viewModel.SelectedTribe = tribe;
                    }
                }
            }

            if (!String.IsNullOrEmpty(profile.FunctionId))
            {
                viewModel.SelectedFunction = structureController.Structure.Functions[profile.FunctionId];
            }

            if (String.IsNullOrEmpty(profile.ImageId))
            {
                viewModel.Image = ImageSource.FromFile("avatar.jpg");
            }
            else
            {
                imageApi.Get(profile.ImageId).Subscribe(image => {
                    viewModel.Image = ImageSource.FromStream(() => new MemoryStream(image.Bytes));
                }, cts.Token);
            }
        }

        async void SaveAsync(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            if(string.IsNullOrEmpty(viewModel.Firstname) || string.IsNullOrEmpty(viewModel.Lastname) || viewModel.SelectedTribe == null || viewModel.SelectedFunction == null || viewModel.WoodbadgeCount < 0)
            {
                viewModel.IsBusy = false;
                DependencyService.Get<IToastMessage>().LongAlert("Du musst alle mit * gekennzeichneten Felder ausfüllen!");
                return;
            }

            if (removeImage)
            {
                model.ImageId = "";
            }

            if (newImage != null)
            {
                ImageModel image = new ImageModel(Guid.NewGuid().ToString());
                image.Bytes = newImage;

                await imageApi.PutAsync(image);
                model.ImageId = image.Id;
            }

            viewModel.WriteToModel(model);
            await profileApi.UpdateAsync(model);
            await Navigation.PopAsync();
            DependencyService.Get<IToastMessage>().ShortAlert("Profil gespeichert");
            viewModel.IsBusy = false;
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

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
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

            MediaFile image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() {
                AllowCropping = true,
                DefaultCamera = CameraDevice.Rear,
                MaxWidthHeight = 300,
                PhotoSize = PhotoSize.Small,
                Directory = "profile",
                Name = model.Id + ".jpg"
            });

            if (image == null)
            {
                return;
            }

            ImageCropPage cropPage = new ImageCropPage(Container);
            cropPage.OnCropResult += (result) =>
            {
                viewModel.IsBusy = true;

                newImage = result;
                viewModel.Image = ImageSource.FromStream(() => new MemoryStream(result));
                removeImage = false;
                viewModel.IsBusy = false;
            };
            await Navigation.PushAsync(cropPage);
            await Task.Run(() => cropPage.SetSource(ImageSource.FromStream(() => image.GetStream())));
            viewModel.IsBusy = false;
        }

        async void PickPhotoAsync()
        {
            viewModel.IsBusy = true;

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

            MediaFile image = await CrossMedia.Current.PickPhotoAsync();

            if (image == null)
            {
                return;
            }

            ImageCropPage cropPage = new ImageCropPage(Container);
            cropPage.OnCropResult += (result) =>
            {
                viewModel.IsBusy = true;

                newImage = result;
                viewModel.Image = ImageSource.FromStream(() => new MemoryStream(result));
                removeImage = false;
                viewModel.IsBusy = false;
            };
            await Navigation.PushAsync(cropPage);
            await Task.Run(() => cropPage.SetSource(ImageSource.FromStream(() => image.GetStream())));
            viewModel.IsBusy = false;
        }

        async void RemovePhotoAsync()
        {
            viewModel.Image = ImageSource.FromFile("avatar.png");
            removeImage = true;
            newImage = null;
        }
    }
}