﻿using System;

namespace Inkore.UI.WPF.Modern.Controls
{
    internal sealed class FlyoutBaseClosingEventArgs : EventArgs
    {
        internal FlyoutBaseClosingEventArgs()
        {
        }

        public bool Cancel
        {
            get => false;
            set => throw new NotImplementedException();
        }
    }
}
