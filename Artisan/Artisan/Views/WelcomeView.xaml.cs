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
using System.Windows.Shapes;
using Artisan.ViewModels;
using Dao.Entities;
using Dao;
using System.Diagnostics;

namespace Artisan.Views
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : Window
    {
        public WelcomeView()
        { 
            InitializeComponent();
            WelcomeScreenViewModel.DoneCompleted += TerminateValidation;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void TerminateValidation(User usr, string errorMessage)
        {
            if(PasswordTextBox1.Password == string.Empty)
            {
                errorMessage += "\n Input a valid password please.";
            }
            Debug.WriteLine(errorMessage);
            if(errorMessage.Length == 0)
            {
                var usrDao = new UserDao("User");
                usr.Password = PasswordTextBox1.Password;
                usrDao.SaveNew(usr);
            }
            else
            {
                //var view = new MessageDialog
                //{
                //    DataContext = new MessageDialogViewModel("Error Message", errorMessage)
                //};

                ////show the dialog
                //var result = DialogHost.Show(view, "RootDialog", OpenEventHandler, ClosingEventHandler);

                MessageBox.Show(errorMessage);
            }
        }

        //private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        //{
        //}
        //private void OpenEventHandler(object sender, DialogOpenedEventArgs eventargs)
        //{
        //}
    }
}
