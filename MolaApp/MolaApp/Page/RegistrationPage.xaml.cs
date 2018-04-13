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
	public partial class RegistrationPage : MolaPage
    {
        public string ScannedId;
        IUserApi userApi;

        public RegistrationPage (ServiceContainer container) : base(container)
		{
			InitializeComponent ();
            userApi = Container.Get<IUserApi>("api/user");
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

                ScannedId = result.Text;

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

        async void RegisterAsync(object sender, EventArgs e)
        {
            UserModel model = new UserModel(ScannedId);
            model.Email = emailEntry.Text;
            model.Password = passwordEntry.Text;

            try
            {
                await userApi.CreateAsync(model);
                await Navigation.PopAsync();
            }
            catch (ConflictException ex)
            {
                if(ex.Field == UserApi.ConflictFieldId)
                {
                    codeErrorLabel.Text = "Der von dir gescannte Code ist bereits für einen anderen Benutzer reserviert!";
                }
                else if(ex.Field == UserApi.ConflictFieldEmail)
                {
                    emailErrorLabel.Text = "Es existiert bereits ein Account mit dieser Email-Adresse";
                }
            }
        }

        async void CancelAsync(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    public class StringEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool invert = false;
            if (parameter is bool)
            {
                invert = (bool)parameter;
            }

            if(string.IsNullOrEmpty(value as string))
            {
                return !invert;
            }
            else
            {
                return invert;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This conversion is not possible");
        }
    }
}