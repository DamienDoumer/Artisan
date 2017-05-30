using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Services
{
    public class ChatUser
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public ChatUser(string name, string ip)
        {
            Name = name;
            IP = ip;
        }

        public override bool Equals(object obj)
        {
            ChatUser usr = obj as ChatUser;
            if (usr != null)
            {
                return usr.IP == this.IP;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
