using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MolaApp.Page
{
	abstract public class MolaPage : ContentPage
	{
        protected ServiceContainer Container { get; }

		public MolaPage (ServiceContainer container)
		{
            Container = container;
		}
	}
}