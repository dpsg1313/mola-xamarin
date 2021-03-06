﻿using MolaApp.Model;
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
        ProfileApi profileApi;

        LoginViewModel viewModel;

        public LoginPage (ServiceContainer container) : base(container)
		{
			InitializeComponent ();

            viewModel = new LoginViewModel();
            BindingContext = viewModel;

            authController = Container.Get<AuthController>("auth");
            profileApi = Container.Get<ProfileApi>("api/profile");
        }

        async void RegisterAsync(object sender, EventArgs e)
        {
            RegistrationPage registrationPage = new RegistrationPage(Container);
            await Navigation.PushAsync(registrationPage);
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

            await Task.Run(() => waitHandle.WaitOne());
        }

        async void LoginAsync(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            UserModel credentials = new UserModel(viewModel.ScannedId);
            credentials.Password = passwordEntry.Text;
            bool success = await authController.LoginAsync(credentials);
            if (success)
            {
                Navigation.InsertPageBefore(new MainPage(Container), this);
                await Navigation.PopAsync();
                DependencyService.Get<IToastMessage>().ShortAlert("Login erfolgreich");
            }
            else
            {
                await DisplayAlert("Login fehlgeschlagen", "Möglicherweise hast du ein falsches Passwort eingegeben oder du hast gerade keine ausreichende Internetverbindung.", "Ok");
            }
            viewModel.IsBusy = false;
        }
    }
}