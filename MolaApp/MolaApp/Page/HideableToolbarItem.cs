﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MolaApp.Page
{
    public class HideableToolbarItem : ToolbarItem
    {
        public HideableToolbarItem() : base()
        {
            this.InitVisibility();
        }

        private async void InitVisibility()
        {
            await Task.Delay(100);
            OnIsVisibleChanged(this, false, IsVisible);
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(HideableToolbarItem), false, propertyChanged: (bindable, oldvalue, newvalue) => OnIsVisibleChanged(bindable, (bool) oldvalue, (bool) newvalue));

        private static void OnIsVisibleChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var item = bindable as HideableToolbarItem;

            if (item.Parent == null)
                return;

            var items = ((ContentPage)item.Parent).ToolbarItems;

            if (newvalue && !items.Contains(item))
            {
                items.Add(item);
            }
            else if (!newvalue && items.Contains(item))
            {
                items.Remove(item);
            }
        }
    }
}
