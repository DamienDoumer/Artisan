using System;
using Dao.Entities;
using Dao;
using Artisan.MVVMShared;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Diagnostics;

namespace Artisan.ViewModels
{
    class WelcomeScreenViewModel : ValidatableBindableBase
    {
        public static event Action<User, string> DoneCompleted;

        /// <summary>
        /// Commands
        /// </summary>
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand DoneCommand { get; private set; }

        /// <summary>
        /// Properties
        /// </summary>
        private string n;
        private string phn;
        private string email;
        User NewUser { get; set; }
        UserDao NewUserDao { get; }
        
        public string PhoneNumber { private get { return phn; } set { phn = value; DoneCommand.RaiseCanExecuteChanged(); } }
        public string Name { get { return n; } set { n = value; DoneCommand.RaiseCanExecuteChanged(); } }
        public string EMail { get { return email; } set { email = value; DoneCommand.RaiseCanExecuteChanged(); } }
        public string Password { get; set; }

        public WelcomeScreenViewModel()
        {
            CloseCommand = new RelayCommand(OnClose, CanClose);
            NewUserDao = new UserDao("User");
            DoneCommand = new RelayCommand(OnDone, CanDone);
            ErrorsChanged += RaiseChange;
            PhoneNumber = "";
            Name = "";
            EMail = "";
        }
        private void RaiseChange(object obj, EventArgs e)
        {
            DoneCommand.RaiseCanExecuteChanged();
        }
        private void OnDone()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (Name == string.Empty)
            {
                errorMessage.Append("Input a valid user name please.");
            }
            ///Uses annotations to validate the input email.
            if (!new EmailAddressAttribute().IsValid(EMail) || EMail == string.Empty)
            {
                errorMessage.Append("\nInput a valid e-mail address please.");
            }
            if(PhoneNumber == string.Empty || !new PhoneAttribute().IsValid(PhoneNumber))
            {
                errorMessage.Append("\nInput a valid phone number please.");
            }

            DoneCompleted?.Invoke(new User(Name, PhoneNumber, EMail, ""), errorMessage.ToString());
            //NewUserDao.SaveNew(new User(Name, PhoneNumber, EMail, ""));
        }
        private bool CanDone()
        {
            return Name.Length > 0 && PhoneNumber.Length > 0 && EMail.Length > 0;
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
