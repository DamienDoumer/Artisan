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
using Messenger.ViewModels;
using System.Windows.Threading;
using System.Diagnostics;

namespace Messenger.Views
{
    /// <summary>
    /// Interaction logic for MessengerView.xaml
    /// </summary>
    public partial class MessengerView : UserControl
    {
        public MessengerView()
        {
            MessengerViewModel.NewArtisanDetected += OnMessengerViewModel_NewArtisanDetected;
            MessengerViewModel.ArtisanRemoved += OnMessengerViewModel_ArtisanRemoved;

            InitializeComponent();

            DataContext = new MessengerViewModel(true);
        }

        public void OnMessengerViewModel_ArtisanRemoved(Services.ChatUser obj)
        {
            this.Dispatcher.Invoke((new Action(() =>
            {
                MessengerViewModel vm = DataContext as MessengerViewModel;
                vm.RemoveArtisan(obj);
            })));
        }

        //Trigger the addition of the new Detected user to the 
        //Observable list since the APP Dispatcher wont work.
        public void OnMessengerViewModel_NewArtisanDetected(Services.ChatUser obj)
        {
            this.Dispatcher.Invoke((new Action(() =>
            {
                MessengerViewModel vm = DataContext as MessengerViewModel;
                vm.AddArtisan(obj);
                Debug.WriteLine(ConnectedUsersListBox.Items.Count);
            })));
        }
    }
}
