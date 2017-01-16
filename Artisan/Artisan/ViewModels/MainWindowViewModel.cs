using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.ViewModels;
using Artisan.MVVMShared;
using System.Diagnostics;

namespace Artisan.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// These commands are to handle the click events of the 
        /// Hamburger Menu.
        /// </summary>
        public RelayCommand<string> NavigationCommand { get; private set; }
        public RelayCommand<string> MainMenuNavCommand { get; private set; }

        private CreateWorkingSessionViewModel createWorkingSessionViewModel = new CreateWorkingSessionViewModel();
        private ManageWorkingSessionsViewModel manageWorkingSessionViewModel = new ManageWorkingSessionsViewModel();
        private ManageEventViewModel manageEventsViewModel = new ManageEventViewModel();
        private CreateEventViewModel createEventViewModel = new CreateEventViewModel();
        private MainMenuViewModel mainMenuViewModel = new MainMenuViewModel();

        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set {SetProperty(ref currentViewModel, value); }
        }

        public MainWindowViewModel()
        {
            NavigationCommand = new RelayCommand<string>(OnNavigation);
            MainMenuNavCommand = new RelayCommand<string>(OnMainMenuNav);
            CurrentViewModel = createEventViewModel;
        }

        private void OnMainMenuNav(string d)
        {
            currentViewModel = mainMenuViewModel;
            Debug.WriteLine(currentViewModel);
        }

        private void OnNavigation(string view)
        {
            switch (view)
            {
                case "main":
                    CurrentViewModel = mainMenuViewModel;
                    break;
                case "manageWorkingSessions":
                    CurrentViewModel = manageWorkingSessionViewModel;
                    break;
                case "manageEvents":
                    CurrentViewModel = manageEventsViewModel;
                    break;
            }
        }
    }
}
