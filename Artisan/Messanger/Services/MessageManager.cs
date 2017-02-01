using Messenger.Services.Messages;
using System.Collections.Generic;
using Seed;
using Seed.Shared;
using Dao.Entities;
using Dao;

namespace Messanger.Services
{
    public class MessageManager
    {
        /// <summary>
        /// This object corresponds to the IP address of each session, 
        /// And its related Messages, While precising iif the user sent or received the message
        /// </summary>
        public Dictionary<string, List<SimpleMessage>> Conversations { get; private set; }
        public Session CurrentSession { get; private set; }
        /// <summary>
        /// This corresponds tho the users who are connected
        /// </summary>
        public Dictionary<string, AuthenticationObject> ConnectedUsers { get; private set; }

        public MessageManager()
        {

        }


    }
}
