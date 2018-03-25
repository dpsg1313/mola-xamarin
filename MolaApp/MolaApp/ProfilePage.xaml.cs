using MolaApp.Model;
using MolaApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MolaApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        protected ProfileModel Profile { get; set; }

		public ProfilePage (ProfileModel profile)
		{
            this.BindingContext = profile;

            InitializeComponent ();
		}
	}
}