using MolaApp.Api;
using MolaApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryPage : MolaPage
	{
        const string DATETIME_FORMAT = "dddd, d.M.yyyy H:mm 'Uhr'";

        HistoryController historyController;
        ProfileApi profileApi;
        ImageApi imageApi;

        public ObservableCollection<HistoryViewModel> History { get; }

        Dictionary<string, HistoryViewModel> _history;

        CancellationTokenSource cts;

        public HistoryPage(ServiceContainer container) : base(container)
		{
			InitializeComponent();

            historyController = Container.Get<HistoryController>("history");
            profileApi = Container.Get<ProfileApi>("api/profile");
            imageApi = Container.Get<ImageApi>("api/image");

            History = new ObservableCollection<HistoryViewModel>();
            _history = new Dictionary<string, HistoryViewModel>();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cts = new CancellationTokenSource();

            foreach (HistoryModel model in historyController.History.OrderBy(m => m.Date, new DateTimeOffsetInvertedComparer()))
            {
                string profileId = model.ProfileId;

                HistoryViewModel viewModel = null;

                if (!_history.TryGetValue(profileId, out viewModel))
                {
                    viewModel = new HistoryViewModel(profileId);
                    viewModel.Label = profileId;
                    viewModel.Date = model.Date.ToString(DATETIME_FORMAT);
                    _history.Add(profileId, viewModel);
                }
                else
                {
                    // Remove and re-add viewModel to get correct ordering
                    History.Remove(viewModel);
                }

                History.Add(viewModel);

                profileApi.Get(profileId).Subscribe(profileModel =>
                {
                    viewModel.Label = profileModel.Name;
                    
                    if(viewModel.ImageId != profileModel.ImageId)
                    {
                        viewModel.ImageId = profileModel.ImageId;
                        void setImage(ImageModel image)
                        {
                            viewModel.Image = ImageSource.FromStream(() => new MemoryStream(image.Bytes));
                        };
                        imageApi.Get(profileModel.ImageId).Subscribe(setImage, cts.Token);
                    }
                }, cts.Token);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            cts.Cancel();
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            string profileId = (e.Item as HistoryViewModel).ProfileId;
            ProfilePage profilePage = new ProfilePage(Container, profileId);
            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(profilePage);
        }

        class DateTimeOffsetInvertedComparer : IComparer<DateTimeOffset>
        {
            public int Compare(DateTimeOffset x, DateTimeOffset y)
            {
                return DateTimeOffset.Compare(y, x);
            }
        }
    }
}