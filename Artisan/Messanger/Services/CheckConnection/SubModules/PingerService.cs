using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Messenger.Services.CheckConnection.InterfacesLibrary;
using System.Diagnostics;
using System.Threading;
using System;

namespace Messenger.Services.CheckConnection.SubModules
{
    public class PingerService : IPinger
    {
        private Ping pinger;
        private List<string> availableIPs;
        private PingOptions pingOptions;
        private List<string> timedOut;
        private object lockObj;
        private int count;

        public List<string> TimedOut { get { return timedOut; } }
        public PingOptions PingOption { get { return pingOptions; } }
        public Ping Pinger { get { return pinger; } }
        public List<string> AvailableIPs { get { return availableIPs; } }
        public string PingMessage { get; set; }
        public short TimeOut { get; set; }

        /// <summary>
        /// Occures when the overall ping is terminated.
        /// </summary>
        /// <param name="available"></param>
        /// <param name="timedOut"></param>
        public delegate void PingTerminatedEventHandler(List<string> available, List<string> timedOut);
        public event PingTerminatedEventHandler PingTerminated;

        public PingerService()
        {
            pinger = new Ping();
            PingMessage = "#4(/:@!^%";
            TimeOut = 1200;
            lockObj = new object();

            pinger.PingCompleted += OnPingCompleted;

            //pinger.PingCompleted += OnPingCompleted;
            availableIPs = new List<string>();
            timedOut = new List<string>();

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            pingOptions = new PingOptions(64, true);
        }

        /// <summary>
        /// Ping the set of addresses given to it.
        /// </summary>
        /// <returns>Available addresses.</returns>
        public void PingAll(List<string> addresses)
        {
            foreach (string address in addresses)
            {
                PingAsync(address);
            }
        }

        /// <summary>
        /// Ping particular address in an async way
        /// </summary>
        /// <param name="IP"></param>
        public void PingAsync(string IP)
        {
            Ping ping = new Ping();
            ping.PingCompleted += OnPingCompleted;

            ping.SendAsync(IP, TimeOut, IP);
        }

        /// <summary>
        /// Ping's a particular IP.
        /// This method is to be used in a syncronous way.
        /// </summary>
        /// <param name="IP">The IP address of the user user you want to ping.</param>
        /// <returns>A string corresponding to state of the pinged user.</returns>
        public IPStatus Ping(string IP)
        {
            PingReply reply = pinger.Send(IP, TimeOut, Encoding.UTF8.GetBytes(PingMessage), pingOptions);

            return reply.Status;
        }

        /// <summary>
        /// This is the asynchrounous method for when a ping terminates.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                lock (lockObj)
                {
                    availableIPs.Add(e.UserState as string);
                }

            }
            else
            if (e.Reply != null && e.Reply.Status == IPStatus.TimedOut)
            {
                lock (lockObj)
                {
                    TimedOut.Add(e.UserState as string);
                }
            }

            count++;

            //if the overall ping is terminated, fire Ping Terminated event.
            if (count == 254)
            {
                if (PingTerminated != null)
                {
                    PingTerminated(availableIPs, timedOut);
                }
            }
        }


        public List<string> PingAll_2(List<string> hosts)
        {
            List<Thread> threads = new List<Thread>();

            List<string> av = new List<string>();

            // Collection of ping replies.
            List<PingReply> pingReplies = new List<PingReply>();

            // Loop through all host names.
            foreach (var host in hosts)
            {
                // Create a new thread.
                Thread thread = new Thread(() =>
                {
                    // Variable to hold the ping reply.
                    PingReply reply = null;

                    // Create a new Ping object and make sure that it's 
                    // disposed after we're finished with it.
                    using (Ping ping = new Ping())
                    {
                        reply = ping.Send(host);

                    }

                    // Get exclusive lock on the pingReplies collection.
                    lock (av)
                    {
                        // Add the ping reply to the collection.
                        //pingReplies.Add(reply);
                        if (reply.Status == IPStatus.Success)
                        {
                            av.Add(reply.Address.ToString());
                        }

                    }

                });

                // Add the newly created thread to the theads collection.
                threads.Add(thread);

                // Start the thread.
                thread.Start();

            }

            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();

            }

            // Calculate and return the average round-trip time.
            return av;

        }

        List<string> IPinger.PingAll(List<string> addresses)
        {
            throw new NotImplementedException();
        }
    }
}
