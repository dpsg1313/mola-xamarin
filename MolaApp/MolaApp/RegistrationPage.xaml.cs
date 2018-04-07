﻿using MolaApp.Model;
using MolaApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MolaApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
        string scannedId;
        UserService userService;

        public RegistrationPage ()
		{
			InitializeComponent ();
            userService = new UserService();
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

            scanButton.Text = "Code erneut scannen";
            scanButton.BackgroundColor = Color.Gray;
            scanLabel.Text = "Du hast deinen Code erfolgreich gescannt. Deine ID ist: " + scannedId.ToString();
        }

        async void CreateAsync(object sender, EventArgs e)
        {
            UserModel model = new UserModel(scannedId);
            model.Email = emailEntry.Text;
            model.Password = passwordEntry.Text;

            try
            {
                await userService.CreateAsync(model);
                // TODO switch to login page
            }
            catch (ConflictException ex)
            {
                if(ex.Field == UserService.ConflictFieldId)
                {
                    codeErrorLabel.Text = "Der von dir gescannte Code ist bereits für einen anderen Benutzer reserviert!";
                }
                else if(ex.Field == UserService.ConflictFieldEmail)
                {
                    emailErrorLabel.Text = "Es existiert bereits ein Account mit dieser Email-Adresse";
                }
            }
        }
    }
}