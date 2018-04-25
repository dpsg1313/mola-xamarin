using MolaApp.Api;
using MolaApp.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageCropPage : MolaPage
    {
        public event CropResultDelegate OnCropResult;

        public delegate void CropResultDelegate(byte[] result);

        LoginViewModel viewModel;

        public ImageCropPage(ServiceContainer container) : base(container)
        {
            InitializeComponent();

            viewModel = new LoginViewModel();
            BindingContext = viewModel;
            viewModel.IsBusy = true;
        }

        public void SetSource(ImageSource source)
        {
            cropView.SetSourceFinished += (object sender, EventArgs e) =>
            {
                viewModel.IsBusy = false;
            };
            cropView.Source = source;
        }

        async void CropAsync()
        {
            viewModel.IsBusy = true;
            var result = await cropView.GetImageAsJpegAsync(quality: 90, maxWidth: 300, maxHeight: 300);

            byte[] buffer;
            using (BinaryReader br = new BinaryReader(result))
            {
                buffer = br.ReadBytes((int)result.Length);
                OnCropResult(buffer);
            }

            await Navigation.PopAsync();
            viewModel.IsBusy = false;
        }
    }
}