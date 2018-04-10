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

        string userId;

        public LoginPage (ServiceContainer container) : base(container)
		{
			InitializeComponent ();
            authController = Container.Get<AuthController>("auth");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (authController.User != null)
            {
                userId = authController.User.Id;
                passwordEntry.Text = authController.User.Password;
            }
        }

        async void RegisterAsync(object sender, EventArgs e)
        {
            RegistrationPage registrationPage = new RegistrationPage(Container);
            await Navigation.PushModalAsync(registrationPage);
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

                userId = result.Text;

                waitHandle.Set();

                // Pop the scanner page
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            };

            // Navigate to our scanner page
            await Navigation.PushModalAsync(scanPage);

            await Task.Run(() => waitHandle.WaitOne());

            scanLabel.Text = userId;
            scanButton.Text = "Anderen Code scannen";
            scanButton.BackgroundColor = Color.Gray;
        }

        async void LoginAsync(object sender, EventArgs e)
        {
            UserModel credentials = new UserModel(userId);
            credentials.Password = passwordEntry.Text;
            bool success = await authController.LoginAsync(credentials, saveLoginSwitch.IsToggled);
            if (success)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Login fehlgeschlagen", "Möglicherweise hast du ein falsches Passwort eingegeben oder du hast gerade keine aureichende Internetverbindung.", "Ok");
            }
        }

        async void CancelAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}