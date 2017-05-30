using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.Services;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;
using Messenger.Services.Messages;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using Messenger.Views;

namespace Messenger.ViewModels
{
    public class MessengerViewModel : BindableBase
    {
        private static MessageManager messageManager;
        private ChatUser currentUser;
        private ObservableCollection<ChatUser> connectedUsers;
        private ObservableCollection<SimpleMessage>currentMessages;
        private string currentMessage;
        //This is used to store the connected users 
        //Even when the user switches views and a new instance is needed
        private static List<ChatUser> connectedUsersStatic;
        /// <summary>
        /// THis tells if the events from the MessageManager have already been subscribed to.
        /// </summary>
        private static bool Subscribed;
        private string connected;

        //Fired when a new Artisan is discovered on the network
        public static event Action<ChatUser> NewArtisanDetected;
        //Fired when an Artisan disconects.
        public static event Action<ChatUser> ArtisanRemoved;
        public string Connected
        {
            get { return connected; }
            set { SetProperty(ref connected, value); }
        }


        ///// <summary>
        ///// This delegate is used to add artisans by pointing to the method found in the 
        ///// View which contain's the Dispatcher
        ///// </summary>
        //public delegate void ArtisanAddition(ChatUser usr);
        //public static ArtisanAddition AddingArtisan;

        ///// <summary>
        ///// This is used to remove artisans using the same trick as AddArtisans
        ///// </summary>
        ///// <param name="usr"></param>
        //public delegate void ArtisanRemoval(ChatUser usr);
        //public static ArtisanRemoval RemovingArtisan;
    
        public static bool MessengerStarted { get; set; }
        public static Dictionary<ChatUser, ObservableCollection<SimpleMessage>> UserChats { get; set; }
        public ObservableCollection<SimpleMessage> CurrentMessages
        {
            get { return currentMessages; }
            set { SetProperty(ref currentMessages, value); }
        }
        public ObservableCollection<ChatUser> ConnectedUsers
        {
            get { return connectedUsers; }
            set { SetProperty(ref connectedUsers, value); }
        }
        public ChatUser CurrentUser
        {
            get { return currentUser; }
            set
            {
                SetProperty(ref currentUser, value);
                if(currentUser != null)
                    CurrentMessages = UserChats[currentUser];
            }
        }
        public string CurrentMessage
        {
            get { return currentMessage; }
            set
            {
                SetProperty(ref currentMessage, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand SendCommand { get; private set; }
        public RelayCommand SendFileCommand { get; private set; }

        /// <summary>
        /// This detect's if the View is the one instantiating this ViewModel
        /// </summary>
        /// <param name="isView"></param>
        public MessengerViewModel(bool isView)
        {
            SendCommand = new RelayCommand(OnSendCommand, CanSend);

            ///If these Events have not ben subscribed to by an instance of the ViewModel, 
            ///then Subbscribe else do nothing
            if(!Subscribed)
            {
                messageManager = MessageManager.Instance;
                messageManager.ArtisanDetected += OnMessageManager_ArtisanDetected;
                messageManager.ResetAll += OnMessageManager_ResetAll;
                messageManager.ConnectivityChanged += OnMessageManager_ConnectivityChanged;
                messageManager.MessageReceived += OnMessageManager_MessageReceived;
                messageManager.ArtisanDisconnected += OnMessageManager_ArtisanDisconnected;

                Subscribed = true;
            }
            

            if(UserChats == null)
            {
                UserChats = new Dictionary<ChatUser, ObservableCollection<SimpleMessage>>();
            }
            if(connectedUsersStatic == null)
            {
                connectedUsersStatic = new List<ChatUser>();
                ConnectedUsers = new ObservableCollection<ChatUser>();
            }
            else
            {
                ConnectedUsers = new ObservableCollection<ChatUser>(connectedUsersStatic);
            }
            currentMessages = new ObservableCollection<SimpleMessage>();
            
            SendFileCommand = new RelayCommand(OnSendFile, CanSendFile);

            if(isView)
            {
                StartMessenger();
            }
        }

        private void OnMessageManager_ArtisanDisconnected(ChatUser obj)
        {
            ArtisanRemoved?.Invoke(obj);
        }

        public static void StartMessenger()
        {
            if (!MessengerStarted)
            {
                messageManager.DetectArtisans();
                MessengerStarted = true;
            }
        }

        private void OnMessageManager_MessageReceived(ChatUser arg1, SimpleMessage arg2)
        {
            DispatchService.Invoke(new Action(() =>
            {
                UserChats[arg1].Add(arg2);
            }));
        }

        private void OnMessageManager_ConnectivityChanged(bool obj)
        {
            throw new NotImplementedException();
        }

        private void OnMessageManager_ResetAll()
        {
            DispatchService.Invoke(new Action(() =>
            {
                CurrentMessages.Clear();
                if (CurrentUser != null)
                {
                    CurrentUser = null;
                }
                UserChats.Clear();
            }));
        }

        private void OnMessageManager_ArtisanDetected(ChatUser obj)
        {
            Debug.WriteLine(NewArtisanDetected == null);
            NewArtisanDetected?.Invoke(obj);
        }

        public void RemoveArtisan(ChatUser obj)
        {
            if(CurrentUser != null)
            {
                if (CurrentUser.IP == obj.IP)
                {
                    if (CurrentMessages != null)
                    {
                        CurrentMessages.Clear();
                    }
                }
            }
            
            if (UserChats.ContainsKey(obj))
            {
                UserChats.Remove(obj);
            }
            if (connectedUsersStatic.Contains(obj))
            {
                connectedUsersStatic.Remove(obj);
                ConnectedUsers.Remove(obj);
            }
        }

        public void AddArtisan(ChatUser obj)
        {
            if (!UserChats.ContainsKey(obj))
            {
                UserChats.Add(obj, new ObservableCollection<SimpleMessage>());
            }
            if (!connectedUsersStatic.Contains(obj))
            {
                connectedUsersStatic.Add(obj);
                ConnectedUsers.Add(obj);
            }
        }

        private void OnSendCommand()
        {
            DispatchService.Invoke(new Action(() =>
            {
                if (CurrentUser != null)
                {
                    if (CurrentMessage != null)
                    {
                        messageManager.SendMessage(CurrentUser, currentMessage);
                        UserChats[CurrentUser].Add(new SimpleMessage(CurrentMessage, DateTime.Now, true));
                        CurrentMessage = null;
                    }
                }
            }));
        }

        private bool CanSend()
        {
            return true;
        }
        public bool CanSendFile()
        {
            return true;
        }
        public void OnSendFile()
        {
        }
    }
}
