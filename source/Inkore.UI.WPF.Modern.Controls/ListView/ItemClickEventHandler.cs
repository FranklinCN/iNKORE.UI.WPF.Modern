﻿using System.Windows;

namespace Inkore.UI.WPF.Modern.Controls
{
    public delegate void ItemClickEventHandler(object sender, ItemClickEventArgs e);

    public sealed class ItemClickEventArgs : RoutedEventArgs
    {
        public ItemClickEventArgs()
        {
        }

        public object ClickedItem { get; internal set; }
    }
}
