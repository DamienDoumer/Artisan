using System.Net.NetworkInformation;

namespace Messenger.Services.CheckConnection
{
    //This class is used to define Delegates which will be used by 
    //Interfaces of this assembly because creation of delegates in interfaces is forbidden
    public class Delegates
    {
        /// <summary>
        /// This is fired when the user's connectivity to a wifi network is down
        /// </summary>
        /// <param name="netInterface">The network interface for this Netchecker.</param>
        /// <param name="isConnected">If connection is up or down.</param>
        public delegate void NetWorkConnectivityChangedEventHandler(NetworkInterface netInterface,
            bool isConnected, string message);

        /// <summary>
        /// This is the delgate to handle the event fired when the IP address of 
        /// the user's PC changes.
        /// </summary>
        /// <param name="IP"></param>
        public delegate void IPAddressChangeEventHandler(string IP);
    }
}
