using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GPS_Datalogger
{
    public class NMEAData
    {

        public DateTime ReciveSystemTime;
        public double Longitude;
        public double Latitude;
        public byte QualityIndicator;
        public int Satelites;
        public double HDOP;
        public double Altitude;
        public double AgeDGPS;
        public float SpeedKnots;
        public double IMUHeading;
        public double IMURoll;


        public void UpdateData(byte[] data)
        {
            ReciveSystemTime = DateTime.UtcNow;


            Longitude = BitConverter.ToDouble(data, 5);
            Latitude = BitConverter.ToDouble(data, 13);
            QualityIndicator = data[43];
            Satelites = BitConverter.ToUInt16(data, 41);
            HDOP = BitConverter.ToUInt16(data, 44) * 0.01;
            Altitude = BitConverter.ToSingle(data, 37);
            AgeDGPS = BitConverter.ToUInt16(data, 46) * 0.01;
            SpeedKnots = BitConverter.ToSingle(data, 29);
            IMUHeading = BitConverter.ToUInt16(data, 48);
            IMURoll = BitConverter.ToInt16(data, 50);
        }

    }
}
