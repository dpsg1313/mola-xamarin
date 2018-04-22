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
	public partial class PrivacyPolicyPage : MolaPage
	{
		public PrivacyPolicyPage(ServiceContainer container) : base(container)
        {
			InitializeComponent ();
		}
	}
}