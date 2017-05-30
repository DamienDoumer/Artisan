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
        private object clientServer;

        public INetChecker NetChecker { get { return netChecker; } }
        public PingerService Pinger { get { return pinger; } }

        /// <summary>
        /// Event starts when the proccess of checking for connection is initiated
        /// </summary>
        /// <param name="obj">The connection Detector instance.</param>
        /// <param name="networkName">The name of the network.</param>
        public delegate void ConnectionCheckStartedEventHandler(object obj, string networkName);
        public event ConnectionCheckStartedEventHandler ConnectionCheckStarted;

        /// <summary>
        /// This authenticates the caller of this service that of succesful connection establishment
        /// With a user.
        /// </summary>
        /// <param name="thisClass"></param>
        /// <param name="authenticatedUser"></param>
        public delegate void ConnectionEstablishedEventHandler(object thisClass, object authenticatedUser);
        public event ConnectionEstablishedEventHandler ConnectionEstablished;

        /// <summary>
        /// This is fired when the process of searching for connected devices has stopped.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="status"></param>
        public delegate void ConnectionCheckStoppedEventHandler(object obj, string status);
        public event ConnectionCheckStoppedEventHandler ConnectionStopped;

        /// <summary>
        /// Fired when the Pinging process is terminated.
        /// </summary>
        /// <param name="av"></param>
        /// <param name="timeO"></param>
        public delegate void PingAllTerminatedEventHandler(List<string> av, List<string> timeO);
        public event PingAllTerminatedEventHandler PingAllTerminated;

        public ConnectionCheckerService(object comTCPobject)
        {
            //authenticator = new Authenticator();
            netChecker = new WirelessNetCheckerService();
            pinger = new PingerService();
            pinger.PingTerminated += OnPingAllTerminated;

            //This is the CLient Server object used to handle TCP messaging.
            clientServer = comTCPobject;
        }

        public ConnectionCheckerService(INetChecker netCheckerType, object comTCPobject)
        {
            netChecker = netCheckerType;
            pinger = new PingerService();
            clientServer = comTCPobject;
        }

        /// <summary>
        /// Runs a time span for an authentication proccess.
        /// This method runs on a different thread and ends the thread
        /// If user authenticates or not.
        /// </summary>
        public void ResponseTimeSpan()
        {

        }

        /// <summary>
        /// Check's all DMera users present on the network
        /// </summary>
        /// <returns>Returns a list of all DMera connected Users</returns>
        public List<object> CheckConnectedDevices()
        {
            return null;
        }

        /// <summary>
        /// Checks if user is on network.
        /// </summary>
        /// <param name="user">The user to be checked</param>
        /// <returns>true if user is on network else return false</returns>
        public Boolean IsUserConnected(Object user)
        {
            return true;
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
            if (PingAllTerminated != null)
            {
                PingAllTerminated(av, timO);
            }
        }
    }
}
