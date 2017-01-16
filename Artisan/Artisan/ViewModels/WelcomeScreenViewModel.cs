using System;
using Dao.Entities;
using Dao;
using Artisan.MVVMShared;

namespace Artisan.ViewModels
{
    class WelcomeScreenViewModel : BindableBase
    {
        /// <summary>
        /// Commands
        /// </summary>
        public RelayCommand CloseCommand { get; private set; }

        /// <summary>
        /// Properties
        /// </summary>
        private string n;
        User NewUser { get; set; }
        UserDao NewUserDao { get; }
        public string PhoneNumber { private get; set; }
        public string Name { get { return n; } set { n = value; } }
        public string EMail { get; set; }
        public string Paword { get; set; }

        public WelcomeScreenViewModel()
        {
            CloseCommand = new RelayCommand(OnClose, CanClose);
            NewUserDao = new UserDao("User");
        }

        /// <summary>
        /// This method is called when the close button is pressed
        /// </summary>
        private void OnClose()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// This is the peace of code used to determine if the user can close the app or not
        /// </summary>
        /// <returns></returns>
        private bool CanClose()
        {
            return true;
        }
    }
}
