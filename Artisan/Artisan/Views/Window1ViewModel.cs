using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;
using Artisan.ViewModels;
using TimeManager.ViewModels;
using System.Diagnostics;

namespace Artisan.Views
{
    public class Window1ViewModel : BindableBase
    {
        private BindableBase currentViewModel;
        public RelayCommand<string> Nav { get; private set; }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }
        public Window1ViewModel()
        {
            CurrentViewModel = new CreateEventViewModel();
            Nav = new RelayCommand<string>(Navigate);
        }

        public void Navigate(string nav)
        {
            currentViewModel = new MainMenuViewModel();
            Debug.WriteLine(currentViewModel.ToString());
        }

        public bool Nav2()
        {
            currentViewModel = new MainMenuViewModel();
            return true;
        }
    }
}
