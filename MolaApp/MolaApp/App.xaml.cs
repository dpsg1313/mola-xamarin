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
            MainPage = new LoadingPage();
            InitializeComponent();
        }

		protected override async void OnStart ()
		{
            //ProfileApi profileApi = new ProfileApi();
            //UserApi userApi = new UserApi();
            //AdventureApi adventureApi = new AdventureApi();
            //StructureApi structureApi = new StructureApi();

            IProfileApi profileApi = new Api.Stub.ProfileApi();
            IUserApi userApi = new Api.Stub.UserApi();
            IAdventureApi adventureApi = new Api.Stub.AdventureApi();
            Api.Stub.StructureApi structureApi = new Api.Stub.StructureApi();
            await structureApi.InitAsync();

            AuthController authController = new AuthController(userApi);
            await authController.InitAsync();

            StructureController structureController = new StructureController(structureApi);
            await structureController.InitAsync();

            ProfileRepository profileRepo = new ProfileRepository(profileApi);
            await profileRepo.InitAsync();
            
            container = new ServiceContainer();
            container.Add("auth", authController);
            container.Add("structure", structureController);
            container.Add("repository/profile", profileRepo);
            container.Add("api/profile", profileApi);
            container.Add("api/user", userApi);
            container.Add("api/adventure", adventureApi);
            container.Add("api/structure", structureApi);

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
