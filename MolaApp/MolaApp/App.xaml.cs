using MolaApp.Api;
using MolaApp.Page;
using MolaApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MolaApp
{
	public partial class App : Application
	{
        private ServiceContainer container;

		public App ()
		{
			InitializeComponent();

            MainPage = new LoadingPage();
        }

		protected override async void OnStart ()
		{
            //ProfileApi profileApi = new ProfileApi();
            //UserApi userApi = new UserApi();
            //AdventureApi adventureApi = new AdventureApi();

            IProfileApi profileApi = new Api.Stub.ProfileApi();
            IUserApi userApi = new Api.Stub.UserApi();
            IAdventureApi adventureApi = new Api.Stub.AdventureApi();

            AuthController authController = new AuthController(userApi);
            await authController.InitAsync();
            
            ProfileRepository profileRepo = new ProfileRepository(profileApi);
            await profileRepo.InitAsync();
            
            container = new ServiceContainer();
            container.Add("auth", authController);
            container.Add("repository/profile", profileRepo);
            container.Add("api/profile", profileApi);
            container.Add("api/user", userApi);
            container.Add("api/adventure", adventureApi);

            if (authController.IsLoggedIn)
            {
                MainPage = new NavigationPage(new MainPage(container));
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage(container));
            }
            
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
