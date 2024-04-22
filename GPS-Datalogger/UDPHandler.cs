using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GPS_Datalogger
{
    public class UDPHandler
    {
        static UdpClient udpClient = new UdpClient();

        public static event Action? NEMAUpdated;

        static private bool ListenToUDP = true;
        public static NMEAData NMEAData = new NMEAData();

        public static void StartListen()
        {
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 15555));
            udpClient.EnableBroadcast = true;
            IPEndPoint from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {

                while (ListenToUDP)
                {
                    Byte[] data = udpClient.Receive(ref from);

                    if (data[3] == 0xD6)
                    {
                        NMEAData.UpdateData(data);
                        TriggerNEMAUpdated();
                    }
                }
            });


        }

        public static void TriggerNEMAUpdated()
        {
            if (NEMAUpdated != null)
            {
                NEMAUpdated.Invoke();
            }
        }


    }
}
