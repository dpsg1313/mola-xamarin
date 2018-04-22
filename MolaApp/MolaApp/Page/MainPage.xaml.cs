using MolaApp.Api;
using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MolaApp.Page
{
	public partial class MainPage : MolaPage
	{
        IProfileApi profileApi;

        AuthController authController;
        HistoryController historyController;

        string scannedId;

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

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}

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

            Console.WriteLine(scannedId);
            await historyController.SetScannedNow(scannedId);

            ProfilePage profilePage = new ProfilePage(Container, scannedId);
            await Navigation.PushAsync(profilePage);
        }
    }
}
