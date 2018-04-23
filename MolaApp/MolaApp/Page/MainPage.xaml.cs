using MolaApp.Api;
using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MolaApp.Page
{
	public partial class MainPage : MolaPage
	{
        Regex uuidValidator = new Regex(@"^[a-f0-9]{8}-[a-f0-9]{4}-4[a-f0-9]{3}-[89aAbB][a-f0-9]{3}-[a-f0-9]{12}$", RegexOptions.IgnoreCase);

        IProfileApi profileApi;

        AuthController authController;
        HistoryController historyController;

		public MainPage(ServiceContainer container) : base(container)
		{
			InitializeComponent();
            profileApi = Container.Get<IProfileApi>("api/profile");
            authController = Container.Get<AuthController>("auth");
            historyController = Container.Get<HistoryController>("history");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        async void BookmarksAsync(object sender, EventArgs e)
        {
            BookmarksPage bookmarksPage = new BookmarksPage(Container);
            await Navigation.PushAsync(bookmarksPage);
        }

        async void HistoryAsync(object sender, EventArgs e)
        {
            HistoryPage historyPage = new HistoryPage(Container);
            await Navigation.PushAsync(historyPage);
        }

        async void EditProfileAsync(object sender, EventArgs e)
        {
            ProfileModel profile = await profileApi.GetAsync(authController.AuthToken.UserId);
            ProfileEditPage profilePage = new ProfileEditPage(Container);
            await profilePage.SetModel(profile);
            await Navigation.PushAsync(profilePage);
        }

        async void MyProfileAsync(object sender, EventArgs e)
        {
            await ShowProfile(authController.AuthToken.UserId);
        }

        async Task ShowProfile(string profileId)
        {
            ProfilePage profilePage = new ProfilePage(Container, profileId);
            await Navigation.PushAsync(profilePage);
        }

        async void LogoutAsync(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Logout", "Willst du dich wirklich ausloggen?", "Ja", "Nein");
            if (result)
            {
                await authController.LogoutAsync();
                Navigation.InsertPageBefore(new LoginPage(Container), this);
                await Navigation.PopAsync();
            }
        }

        async void ScanAsync(object sender, EventArgs e)
        {
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<ZXing.BarcodeFormat>() {
                ZXing.BarcodeFormat.QR_CODE
            };
            var scanPage = new ZXingScannerPage(options);

            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            string scannedId = null;
            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                scannedId = result.Text;
                waitHandle.Set();

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            };

            // Navigate to our scanner page
            await Navigation.PushModalAsync(scanPage);
            await Task.Run(() => waitHandle.WaitOne());

            if(scannedId == null)
            {
                return;
            }

            if(!uuidValidator.IsMatch(scannedId))
            {
                DependencyService.Get<IToastMessage>().ShortAlert("Ungültiger Code!");
            }

            if(scannedId != authController.AuthToken.UserId)
            {
                await historyController.SetScannedNow(scannedId);
            }

            await ShowProfile(scannedId);
        }
    }
}
