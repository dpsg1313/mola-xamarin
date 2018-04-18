using MolaApp.Model;
using MolaApp.Repository;
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
	public partial class ProfilePage : MolaPage
    {
        protected ProfileModel Profile { get; set; }

		public ProfilePage (ServiceContainer container, ProfileModel profile) : base(container)
		{
            this.BindingContext = profile;

            Title = profile.Name;

            InitializeComponent ();
		}
	}
}