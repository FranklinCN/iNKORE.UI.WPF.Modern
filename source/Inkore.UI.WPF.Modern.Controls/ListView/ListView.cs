﻿using System.Windows;
using System.Windows.Controls;

namespace Inkore.UI.WPF.Modern.Controls
{
    public class ListView : ListViewBase
    {
        static ListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));
        }

        public ListView()
        {
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ListViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListViewItem();
        }
    }
}
