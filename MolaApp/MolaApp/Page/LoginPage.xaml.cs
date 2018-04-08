using MolaApp.Model;
using MolaApp.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : MolaPage
	{
        AuthController authController;

        UserModel user;
        

        public LoginPage (ServiceContainer container) : base(container)
		{
			InitializeComponent ();
            authController = Container.Get<AuthController>("auth");
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

                user = new UserModel(result.Text);

                waitHandle.Set();

                // Pop the scanner page
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            };

            // Navigate to our scanner page
            await Navigation.PushModalAsync(scanPage);

            await Task.Run(() => waitHandle.WaitOne());

            scanButton.Text = "Anderen Code scannen";
            scanButton.BackgroundColor = Color.Gray;
        }

        async void LoginAsync(object sender, EventArgs e)
        {
            bool success = await authController.LoginAsync(user);
            if (success)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Login fehlgeschlagen", "Möglicherweise hast du ein falsches Passwort eingegeben oder du hast gerade keine aureichende Internetverbindung.", "Ok");
            }
        }
    }
}