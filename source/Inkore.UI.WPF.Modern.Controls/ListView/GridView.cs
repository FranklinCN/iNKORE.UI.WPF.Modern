﻿using System;
using System.Windows;
using System.Windows.Controls;
using Inkore.UI.WPF.Modern.Controls.Primitives;

namespace Inkore.UI.WPF.Modern.Controls
{
    public class GridView : ListViewBase
    {
        static GridView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GridView), new FrameworkPropertyMetadata(typeof(GridView)));
        }

        public GridView()
        {
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is GridViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GridViewItem();
        }
    }
}
