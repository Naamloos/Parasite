using Parasite.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Controller
{
    public class Requester
    {
        private int _port;

        public Requester(int port)
        {
            this._port = port;
        }

        public Response DoRequest(string ip, Request input)
        {
            using(TcpClient client = new TcpClient(ip, this._port))
            {
                var str = client.GetStream();
                byte[] req = Encoding.UTF8.GetBytes(input.ToString());
                str.Write(req, 0, req.Length);

                var resp = new byte[2048];
                str.Read(resp, 0, 2048);
                var stringresp = Encoding.UTF8.GetString(resp);
                var response = Response.FromString(stringresp.Replace("\0", ""));
                return response;
            }
        }
    }
}
