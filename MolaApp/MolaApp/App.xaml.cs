using Akavache;
using Amazon;
using Amazon.S3;
using MolaApp.Api;
using MolaApp.Page;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace MolaApp
{
	public partial class App : Application
	{
        private const string AWS_ACCESS_KEY = "AKIAJ7BWVCEH2E2M4K3Q";
        private const string AWS_SECRET_KEY = "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";
        
        private static string baseUrl = "https://mola-api.dpsg1313.de/v1/";
        //private static string baseUrl = "http://localhost:8888/v1/";

        private ServiceContainer container;

		public App ()
		{
            BlobCache.ApplicationName = "de.dpsg1313.mola";

            MainPage = new LoadingPage();
            InitializeComponent();
        }

		protected override async void OnStart ()
		{
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.MaxResponseContentBufferSize = 256000;

            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;

            AWSConfigs.AWSRegion = "eu-central-1";
            AWSConfigsS3.UseSignatureVersion4 = true;

            IAmazonS3 s3Client = new AmazonS3Client(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.EUCentral1);

            ProfileApi profileApi = new ProfileApi(httpClient);
            UserApi userApi = new UserApi(httpClient);
            AdventureApi adventureApi = new AdventureApi(httpClient);
            ImageApi imageApi = new ImageApi(s3Client);
            //StructureApi structureApi = new StructureApi(httpClient);


            //IProfileApi profileApi = new Api.Stub.ProfileApi();
            //IUserApi userApi = new Api.Stub.UserApi();
            //IAdventureApi adventureApi = new Api.Stub.AdventureApi();
            //Api.Stub.ImageApi imageApi = new Api.Stub.ImageApi();
            Api.Stub.StructureApi structureApi = new Api.Stub.StructureApi();
            await structureApi.InitAsync();
            

            AuthController authController = new AuthController(userApi);
            authController.TokenChanged += (object sender, EventArgs e) =>
            {
                if (authController.IsLoggedIn)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authController.AuthToken.Token);
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = null;
                }
            };
            await authController.InitAsync();

            StructureController structureController = new StructureController(structureApi);
            await structureController.InitAsync();

            BookmarkController bookmarkController = new BookmarkController();
            await bookmarkController.InitAsync();

            HistoryController historyController = new HistoryController();
            await historyController.InitAsync();

            container = new ServiceContainer();
            container.Add("auth", authController);
            container.Add("structure", structureController);
            container.Add("bookmark", bookmarkController);
            container.Add("history", historyController);
            container.Add("api/profile", profileApi);
            container.Add("api/user", userApi);
            container.Add("api/adventure", adventureApi);
            container.Add("api/structure", structureApi);
            container.Add("api/image", imageApi);

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
