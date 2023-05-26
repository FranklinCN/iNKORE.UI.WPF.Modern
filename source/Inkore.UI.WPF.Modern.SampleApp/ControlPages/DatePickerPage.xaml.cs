﻿using Inkore.UI.WPF.Modern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inkore.UI.WPF.Modern.SampleApp.ControlPages
{
    /// <summary>
    /// Interaction logic for DatePickerPage.xaml
    /// </summary>
    public partial class DatePickerPage
    {
        public DatePickerPage()
        {
            InitializeComponent();
        }

        private void BlackoutDatesInPast(object sender, RoutedEventArgs e)
        {
            datePicker.BlackoutDates.AddDatesInPast();
        }
    }
}
