﻿using MahApps.Metro.Controls;
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
using Artisan.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace Artisan.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel.DiaologNeeded += DisplayMessage;
        }

        private async void DisplayMessage(string message)
        {
            var controller = await this.ShowMessageAsync("Hint.", message);
        }
    }
}
