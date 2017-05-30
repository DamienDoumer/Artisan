using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Services.Messages
{
    /// <summary>
    /// This represents a simple text message
    /// </summary>
    public class SimpleMessage
    {
        /// <summary>
        /// Tells if The user is the one who sent the current message
        /// </summary>
        public bool ISent { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; }

        public SimpleMessage(string text, DateTime time, bool iSent)
        {
            TimeStamp = time;
            Text = text;
            ISent = iSent;
        }
    }
}
