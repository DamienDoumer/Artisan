using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.MVVMShared;

namespace TimeManager.ViewModels
{
    public class ManageWorkingSessionsViewModel : BindableBase
    {
        public static event Action CreateWorkingSessionCommand;

        public RelayCommand CreateCommand { get; private set; }

        public ManageWorkingSessionsViewModel()
        {
            CreateCommand = new RelayCommand(OnCreate);
        }

        public void OnCreate()
        {
            CreateWorkingSessionCommand?.Invoke();
        }
    }
}
