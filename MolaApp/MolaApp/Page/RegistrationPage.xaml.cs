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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : MolaPage
    {
        IUserApi userApi;
        IProfileApi profileApi;
        AuthController authController;

        LoginViewModel viewModel;

        public RegistrationPage (ServiceContainer container) : base(container)
		{
			InitializeComponent ();

            viewModel = new LoginViewModel();
            BindingContext = viewModel;

            userApi = Container.Get<IUserApi>("api/user");
            profileApi = Container.Get<IProfileApi>("api/profile");
            authController = Container.Get<AuthController>("auth");
        }

        async void ShowPrivacyPolicy(object sender, EventArgs e)
        {
            PrivacyPolicyPage ppPage = new PrivacyPolicyPage(Container);
            await Navigation.PushAsync(ppPage);
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

                viewModel.ScannedId = result.Text;

                waitHandle.Set();

                // Pop the scanner page
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                });
            };

            // Navigate to our scanner page
            await Navigation.PushModalAsync(scanPage);
        }

        async Task RegisterAsync(object sender, EventArgs e)
        {
            if(passwordEntry.Text.Length < 4)
            {
                await DisplayAlert("Fehler", "Das Passwort muss mindestens 4 Zeichen lang sein!", "Ok");
                return;
            }

            if (passwordEntry.Text != passwordConfirmEntry.Text)
            {
                await DisplayAlert("Fehler", "Die eingegebenen Passwörter stimmen nicht überein!", "Ok");
                return;
            }

            viewModel.IsBusy = true;

            UserModel model = new UserModel(viewModel.ScannedId);
            model.Password = passwordEntry.Text;

            try
            {
                await userApi.CreateAsync(model);
                await Navigation.PopAsync();
                DependencyService.Get<IToastMessage>().LongAlert("Account erstellt. Jetzt musst du dich noch einloggen!");
            }
            catch (ConflictException conflictEx)
            {
                await DisplayAlert("Dieb! ;-)", "Der von dir gescannte Code ist bereits für einen anderen Benutzer reserviert!", "Ok");
            }
            catch(InvalidCodeException codeEx)
            {
                await DisplayAlert("Fehler", "Es sind nur offizielle Aufnäher erlaubt!", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", "Möglicherweise hast du gerade keine ausreichende Internetverbindung. Bitte versuche es an einem anderen Ort erneut!", "Ok");
            }
            viewModel.IsBusy = false;
        }
    }
}