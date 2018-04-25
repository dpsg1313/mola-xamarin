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
	public partial class BookmarksPage : MolaPage
	{
        BookmarkController bookmarkController;
        ProfileApi profileApi;
        ImageApi imageApi;

        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }

        Dictionary<string, BookmarkViewModel> _bookmarks;

        CancellationTokenSource cts;

        public BookmarksPage(ServiceContainer container) : base(container)
		{
			InitializeComponent();

            bookmarkController = Container.Get<BookmarkController>("bookmark");
            profileApi = Container.Get<ProfileApi>("api/profile");
            imageApi = Container.Get<ImageApi>("api/image");

            Bookmarks = new ObservableCollection<BookmarkViewModel>();
            _bookmarks = new Dictionary<string, BookmarkViewModel>();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cts = new CancellationTokenSource();

            foreach (string profileId in bookmarkController.ProfileIds)
            {
                BookmarkViewModel viewModel = null;

                if (!_bookmarks.TryGetValue(profileId, out viewModel))
                {
                    viewModel = new BookmarkViewModel(profileId);
                    viewModel.Label = profileId;
                    _bookmarks.Add(profileId, viewModel);
                    Bookmarks.Add(viewModel);
                }

                profileApi.Get(profileId).Subscribe(model =>
                {
                    viewModel.Label = model.Name;

                    void setImage(ImageModel image)
                    {
                        viewModel.Image = ImageSource.FromStream(() => new MemoryStream(image.Bytes));
                    };
                    imageApi.Get(model.ImageId).Subscribe(setImage, cts.Token);
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
            string profileId = (e.Item as BookmarkViewModel).ProfileId;
            ProfilePage profilePage = new ProfilePage(Container, profileId);
            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(profilePage);
        }
    }
}