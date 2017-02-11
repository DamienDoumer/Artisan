using System;
using System.Net.NetworkInformation;

namespace Messenger.Services.CheckConnection.InterfacesLibrary
{
    public interface INetChecker
    {
        event Delegates.NetWorkConnectivityChangedEventHandler NetworkConnectivityChanged;
        event Delegates.IPAddressChangeEventHandler IPAddressChanged;

        NetworkInterface NetInterface { get; }

        string GetIPAddress();
        string GetNetworkName();
        bool IsConnectedToNetwork();
        bool IsInternetAvailable();
        NetworkInterface GetNetworkInterface();
        void NetConnectionChange(object obj, EventArgs e);
    }
}
