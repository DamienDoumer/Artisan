using MahApps.Metro.Controls;
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
using Dao.Entities;
using System.Collections.ObjectModel;
using Dao;

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
            MainWindowViewModel.DiaologNeeded += OnDisplayMessage;
            MainWindowViewModel.DeleteReponse += OnDeleteResponse;
        }

        private async void OnDisplayMessage(string message)
        {
            MessageDialogResult controller = await this.ShowMessageAsync("Hint", message);
        }
        public async Task<bool> YesOrNoWarning(string message)
        {
            MessageDialogResult result = await this.ShowMessageAsync("Beware", message, 
                MessageDialogStyle.AffirmativeAndNegative);

            if(result == MessageDialogResult.Affirmative)
            {
                return true;
            }

            return false;
        }
        private async void OnDeleteResponse(TimeEntity wrk, object list)
        {
            if (wrk.GetType() == new WorkingSession().GetType())
            {
                if (await YesOrNoWarning("Are you sure you want to delete this working session ?."))
                {
                    WorkingSession wrk2 = wrk as WorkingSession;
                    ObservableCollection<WorkingSession> list2 = list as ObservableCollection<WorkingSession>;
                    new WorkingSessionDao("WorkingSession").Delete(wrk2);
                    list2.Remove(wrk2);
                    return;
                }
            }
            else
            if(await YesOrNoWarning("Are you sure you want to delete this Event?."))
            {
                Event evt = wrk as Event;
                ObservableCollection<Event> evtList = list as ObservableCollection<Event>;
                new EventDao("Event").Delete(evt);
                evtList.Remove(evt);
            }
        }
       
    }
}
