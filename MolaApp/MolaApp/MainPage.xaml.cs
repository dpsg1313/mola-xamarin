using MolaApp.Model;
using MolaApp.Repository;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MolaApp
{
	public partial class MainPage : ContentPage
	{
        string scannedId;

		public MainPage()
		{
			InitializeComponent();
		}

        void Register(object sender, EventArgs e)
        {
            var registerPage = new RegistrationPage();
            Navigation.PushAsync(registerPage);
        }

        async void OnScanAsync(object sender, EventArgs e)
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
            System.Diagnostics.Debug.WriteLine("The modal page is now on screen, hit back button");
            await Task.Run(() => waitHandle.WaitOne());
            System.Diagnostics.Debug.WriteLine("The modal page is dismissed, do something now");

            Console.WriteLine(scannedId);

            randomText.Text = scannedId;

            ProfileRepository repo = new ProfileRepository();
            System.Diagnostics.Debug.WriteLine("Init Repo");
            await repo.InitAsync();
            System.Diagnostics.Debug.WriteLine("Get Profile from Repo");
            ProfileModel profile = await repo.GetAsync(scannedId);

            if(profile == null)
            {
                // show error message
            }

            var profilePage = new ProfilePage(profile);

            await Navigation.PushAsync(profilePage);
        }
    }
}
