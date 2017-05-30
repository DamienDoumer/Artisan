using System;
using System.Collections.Generic;
using Messenger.Services.CheckConnection.SubModules;
using Messenger.Services.CheckConnection.InterfacesLibrary;
//using Language.English.Modules;
using System.Diagnostics;
using System.Threading;

namespace Messenger.Services.CheckConnection
{
    public class ConnectionCheckerService
    {
        private INetChecker netChecker;
        private PingerService pinger;

        public INetChecker NetChecker { get { return netChecker; } }
        public PingerService Pinger { get { return pinger; } }
        
        /// <summary>
        /// Fired when the Pinging process is terminated.
        /// </summary>
        /// <param name="av"></param>
        /// <param name="timeO"></param>
        public delegate void PingAllTerminatedEventHandler(List<string> av, List<string> timeO);
        public event PingAllTerminatedEventHandler PingAllTerminated;

        public ConnectionCheckerService(INetChecker netCheckerType)
        {
            netChecker = netCheckerType;
            pinger = new PingerService();
            pinger.PingTerminated += OnPingAllTerminated;
        }

        /// <summary>
        /// Generates the range of ip addresses which the system will test 
        /// for connection.
        /// </summary>
        /// <returns>The list of IP addresses</returns>
        private List<string> GeneratePingIPs(string IP)
        {
            //Splits the IP after the last . is seen.
            string semiIP = IP.Substring(0, IP.LastIndexOf('.') + 1);
            List<string> IPs = new List<string>();
            string testIP = string.Empty;

            for (int i = 0; i < 256; i++)
            {
                testIP = semiIP + i;
                if (testIP != IP)
                {
                    IPs.Add(testIP);
                }
            }
            return IPs;
        }//works

        /// <summary>
        /// Ping's all network devices which are on the 
        /// Same subnet as the user's IP.
        /// </summary>
        /// <returns>Available devices.</returns>
        public void PingThemAll()
        {
            string IP = netChecker.GetIPAddress();

            if (IP != string.Empty)
            {
                Thread thr = new Thread(() =>
                {
                    Pinger.PingAll(GeneratePingIPs(IP));
                });

                thr.Start();
            }
            else
            {
                throw (new Exception("You are not connected to a wifi network."));
            }
        }

        /// <summary>
        /// This is fired when the ping is terminated. And this fires an event handled out
        /// of the object.
        /// </summary>
        /// <param name="av"></param>
        /// <param name="timO"></param>
        private void OnPingAllTerminated(List<string> av, List<string> timO)
        {
            PingAllTerminated?.Invoke(av, timO);
        }
    }
}
