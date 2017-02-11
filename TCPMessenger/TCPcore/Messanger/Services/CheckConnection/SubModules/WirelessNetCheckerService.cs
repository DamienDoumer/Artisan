using System;
using Messenger.Services.CheckConnection.InterfacesLibrary;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Messenger.Services.CheckConnection.SubModules
{
    public class WirelessNetCheckerService : INetChecker
    {
        private NetworkInterface netInterface;
        public NetworkInterface NetInterface { get { return netInterface; } }

        //Delegates is my class which stores delegates.
        public event Delegates.NetWorkConnectivityChangedEventHandler NetworkConnectivityChanged;
        //fired when the IP address of the user changes.
        public event Delegates.IPAddressChangeEventHandler IPAddressChanged;

        public WirelessNetCheckerService()
        {
            netInterface = GetNetworkInterface();

            //Event that occures when the network availaibility changes.
            NetworkChange.NetworkAvailabilityChanged += NetConnectionChange;

            NetworkChange.NetworkAddressChanged += IPChanged;
        }

        /// <summary>
        /// Checks if current user is connected to a network
        /// </summary>
        /// <returns>returns true if user is connected else returns false</returns>
        public bool IsConnectedToNetwork()
        {
            return GetNetworkInterface().OperationalStatus == OperationalStatus.Up;
        }

        /// <summary>
        /// Get's the name of the network on which the current user is connected.
        /// </summary>
        /// <returns>Network name.</returns>
        public string GetNetworkName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// queries the current IP of the user.
        /// </summary>
        /// <returns>The IP address of the user.</returns>
        public string GetIPAddress()
        {
            NetworkInterfaceType type = netInterface.NetworkInterfaceType;
            string myIp = "";
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < allNetworkInterfaces.Length; i++)
            {
                NetworkInterface networkInterface = allNetworkInterfaces[i];
                if (networkInterface.NetworkInterfaceType == type && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            myIp = ip.Address.ToString();
                        }
                    }
                }
            }
            return myIp;
        }

        /// <summary>
        /// Checks if internet connection is available.
        /// </summary>
        /// <returns>return true else return false.</returns>
        public bool IsInternetAvailable()
        {
            try
            {
                //try to connect to google if it succeed's then there is internet else there is no internet
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }//Works

        public NetworkInterface GetNetworkInterface()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface netInterface = null;

            foreach (NetworkInterface n in adapters)
            {
                if (n.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    netInterface = n;
                }
            }

            return netInterface;
        }//works

        
        /// <summary>
        /// This is a call back function which also fires the 
        /// NetworkConnectivityChanged event which the user can handle later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NetConnectionChange(object sender, EventArgs e)
        {
            bool operationalStatus = true;

            netInterface = GetNetworkInterface();

            string message = netInterface.OperationalStatus.ToString();

            //Set the operational status of the network to 
            //false if the user is not Connected to a wifi network
            //else set true
            if (netInterface.OperationalStatus == OperationalStatus.Up)
            {
                operationalStatus = true;
            }
            else
            {
                operationalStatus = false;
            }

            if (NetworkConnectivityChanged != null)
            {
                NetworkConnectivityChanged(NetInterface, operationalStatus, message);
            }
        }//works

        /// <summary>
        /// Callback function for the IP address change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IPChanged(object sender, EventArgs e)
        {
            IPAddressChanged?.Invoke(GetIPAddress());
        }
    }
}
