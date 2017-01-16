using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManager.ViewModels;
using Artisan.MVVMShared;
using System.Diagnostics;
using Dao.Entities;

namespace Artisan.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// These commands are to handle the click events of the 
        /// Hamburger Menu.
        /// </summary>
        public RelayCommand MainMenuSwitchCommand { get; private set; }
        public RelayCommand ManageEventSwitchCommand { get; private set; }
        public RelayCommand ManageWorkingSessionSwitchCommand { get; private set; }

        CreateEventViewModel createEventViewModel = new CreateEventViewModel();
        MainMenuViewModel mainMenuViewModel = new MainMenuViewModel();
        ManageEventViewModel manageEventViewModel = new ManageEventViewModel();

        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }


        public MainWindowViewModel()
        {
            MainMenuSwitchCommand = new RelayCommand(OnMainMenuViewSwitch);
            ManageEventSwitchCommand = new RelayCommand(OnManageEventViewSwitch);
            CurrentViewModel = createEventViewModel;
        }

        private void OnMainMenuViewSwitch()
        {
            CurrentViewModel = createEventViewModel;
        }
        private void OnManageEventViewSwitch()
        {
            CurrentViewModel = manageEventViewModel;
        }
        private void OnManageWorkingSessionViewSwitch()
        {

        }
        private void OnSettingsViewSwitch()
        {

        }
        private void OnMessagesViewSwitch()
        {

        }
    }
}
