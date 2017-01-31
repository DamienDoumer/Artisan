using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Messenger.Services.CheckConnection.InterfacesLibrary
{
    public interface IPinger
    {
        List<string> PingAll(List<string> addresses);
        IPStatus Ping(string IP);
    }
}
