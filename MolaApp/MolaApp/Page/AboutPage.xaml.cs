using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : MolaPage
	{
		public AboutPage(ServiceContainer container) : base(container)
        {
			InitializeComponent ();
		}

        async void ShowPrivacyPolicy(object sender, EventArgs e)
        {
            PrivacyPolicyPage ppPage = new PrivacyPolicyPage(Container);
            await Navigation.PushAsync(ppPage);
        }
    }
}