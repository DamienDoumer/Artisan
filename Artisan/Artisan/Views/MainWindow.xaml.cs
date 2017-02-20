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
using Artisan.Views.Notifications;
using TimeManager.ViewModels;
using System.Diagnostics;

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
            MainWindowViewModel.DisplayTimeArrivedNotification += MainWindowViewModel_DisplayNotification;
            MainWindowViewModel.DisplayNotificationMessageBox += OnMainWindowViewModel_DisplayNotification1;
            MainWindowViewModel.CancelWorkingSession += OnMainWindowViewModel_CancelWorkingSession;
        }

        private async void OnMainWindowViewModel_CancelWorkingSession(string obj)
        {
            if (await YesOrNoWarning(obj))
            {
                new WorkingSessionDao() { }.SaveAsDoneWorkingSession(InWorkingSessionViewModel.MainWorkingSession);

                foreach (Dao.Entities.Task t in InWorkingSessionViewModel.MainWorkingSession.Tasks)
                {
                    Debug.WriteLine(t.Accomplished);

                    new TaskDao("Task") { }.Update(t);
                }
                InWorkingSessionViewModel.TerminateWoringSession();
                InWorkingSessionViewModel.Terminated = true;

            }
        }

        /// <summary>
        /// Display messageBox notification to the user.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void OnMainWindowViewModel_DisplayNotification1(string arg1, string arg2)
        {
            MessageDialogResult controller = await this.ShowMessageAsync(arg1, arg2);
        }

        /// <summary>
        /// Notify the user when needed
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="isWorkingsession"></param>
        private void MainWindowViewModel_DisplayNotification(string arg1, string arg2, bool isWorkingsession)
        {
            if (isWorkingsession)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    new GrowlNotifications().WorkingSessionTimearrivedNotification(arg1, arg2);
                }));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    new GrowlNotifications().AppointmentTimeArrivedNotification(arg1, arg2);
                }));
            }

            Dispatcher.Invoke((Action)(() =>
            {
                Show();
            }));
        }
        private async void OnDisplayMessage(string message)
        {
            MessageDialogResult controller = await this.ShowMessageAsync("Hint", message);
        }
        public async Task<bool> YesOrNoWarning(string message)
        {
            MessageDialogResult result = await this.ShowMessageAsync("Beware", message,
                MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
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
                    new WorkingSessionDao().Delete(wrk2);
                    list2.Remove(wrk2);
                    return;
                }
            }
            else
            if (await YesOrNoWarning("Are you sure you want to delete this Event?."))
            {
                Event evt = wrk as Event;
                ObservableCollection<Event> evtList = list as ObservableCollection<Event>;
                new EventDao("Event").Delete(evt);
                evtList.Remove(evt);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
        }
    }
}
