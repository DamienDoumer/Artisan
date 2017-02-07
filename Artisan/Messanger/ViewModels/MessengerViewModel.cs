using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.Services;
using Artisan.MVVMShared;
using System.Collections.ObjectModel;
using Messenger.Services.Messages;

namespace Messenger.ViewModels
{
    public class MessengerViewModel : BindableBase
    {
        private MessageManager messenger;
        private ChatUser currentUser;
        ObservableCollection<ChatUser> connectedUsers;
        ObservableCollection<SimpleMessage> currentMessages;
        string message;

        public static event Action<ChatUser, SimpleMessage> MessageNoticationNeeded;

        public RelayCommand SendMessageCommand { get; private set; }

        /// <summary>
        /// The messages which the user is sending
        /// </summary>
        public string CurrentMessage
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        /// <summary>
        /// The current user which the User wants to chat with
        /// </summary>
        public ChatUser CurrentUser
        {
            get { return currentUser; }
            set
            {
                SetProperty(ref currentUser, value);
                ChangeForCurrentChat();
                SendMessageCommand.RaiseCanExecuteChanged();
            }
        }
        /// <summary>
        /// This corresponds tho the users who are connected
        /// </summary>
        public ObservableCollection<ChatUser> ConnectedUsers
        {
            get { return connectedUsers; }
            set { SetProperty(ref connectedUsers, value); }
        }
        /// <summary>
        /// This object corresponds to the IP address of each session, 
        /// And its related Messages, While precising iif the user sent or received the message
        /// </summary>
        public static Dictionary<ChatUser, ObservableCollection<SimpleMessage>> Conversations { get; private set; }
        public ObservableCollection<SimpleMessage> CurrentUserMessages
        {
            get { return currentMessages; }
            set { SetProperty(ref currentMessages, value); }
        }

        public MessengerViewModel()
        {
            Conversations = new Dictionary<ChatUser, ObservableCollection<SimpleMessage>>();
            ConnectedUsers = new ObservableCollection<ChatUser>();
            messenger = new MessageManager();

            ///Subscribe to the messenger's events
            messenger.ChatUserConnected += OnMessenger_ChatUserConnected;
            messenger.ErrorOccured += OnMessenger_ErrorOccured;
            messenger.MessageReceived += OnMessenger_MessageReceived;

            ///Commands
            SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
        }

        private void OnMessenger_MessageReceived(ChatUser chatUser, SimpleMessage message)
        {
            ///I dispatch in other to allow this thread to call UI 
            ///Controls.
            DispatchService.Invoke(new Action(() =>
            {
                Conversations[chatUser].Add(message);
                if (CurrentUser != null)
                {
                    ///If the current chat user is the one who is involved in the conversations, 
                    ///Add it to the current chats, else Notify the user that a message was added.
                    if (CurrentUser.IP == chatUser.IP)
                    { CurrentUserMessages.Add(message); }
                    else
                    {
                        NotifyForMessage(chatUser, message);
                    }
                }
                else
                {
                    NotifyForMessage(chatUser, message);
                }
            }));
        }

        private void OnMessenger_ErrorOccured(ChatUser obj)
        {
            DispatchService.Invoke(new Action(() =>
            {
                Conversations.Remove(obj);
                ConnectedUsers.Remove(obj);
                if (CurrentUser.IP == obj.IP)
                {
                    CurrentUser = null;
                    currentMessages.Clear();
                }
            }
            ));
        }

        private void OnMessenger_ChatUserConnected(ChatUser obj)
        {
            DispatchService.Invoke(new Action(() =>
            {
                Conversations.Add(obj, new ObservableCollection<SimpleMessage>());
                ConnectedUsers.Add(obj);
            }
            ));
        }

        /// <summary>
        /// Changes which should occure when the current user changes
        /// </summary>
        private void ChangeForCurrentChat()
        {
            ///Set the Current Messages to the conversations of the current user.
            CurrentUserMessages = Conversations[CurrentUser];
        }
        /// <summary>
        /// Inform the above layer to notify the user that a new message 
        /// Was received.
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="msg"></param>
        private void NotifyForMessage(ChatUser usr, SimpleMessage msg)
        {
            MessageNoticationNeeded?.Invoke(usr, msg);
        }

        private void SendMessage()
        {
            SimpleMessage msg = new SimpleMessage(CurrentMessage, DateTime.Now, true);
            messenger.SendMessage(currentUser, msg);
            CurrentUserMessages.Add(msg);
            Conversations[CurrentUser].Add(msg);
        }
        private bool CanSendMessage()
        {
            return CurrentUser != null;
        }
    }
}
