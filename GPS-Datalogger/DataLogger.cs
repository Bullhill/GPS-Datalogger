using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_Datalogger
{
    public class DataLogger
    {
        public static void Init()
        {
            if (File.Exists("Database.db"))
            {
                SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder()
                {
                    DataSource = "Database.db",
                    Mode = SqliteOpenMode.ReadWrite
                };
                _conn = new SqliteConnection(builder.ToString());

                _conn.Open();
            }
            else
            {
                CreateNewDb();
            }
        }
        private static SqliteConnection? _conn;
        private static void CreateNewDb()
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder()
            {
                DataSource = "Database.db",
                Mode = SqliteOpenMode.ReadWriteCreate
            };
            _conn = new SqliteConnection(builder.ToString());

            _conn.Open();
            CreateTables();
        }
        private static void CreateTables()
        {
            if (_conn == null)
                throw new Exception("No database connection");

            using SqliteCommand fmd = _conn.CreateCommand();
            fmd.CommandText = @"CREATE TABLE GPSdata (time INTEGER, latitude REAL, longitude REAL, altitude REAL, QualityIndicator INTEGER, Satelites INTEGER, hdop REAL, age REAL, speed REAL, IMUHeading REAL, IMURoll REAL)";
            fmd.CommandType = CommandType.Text;
            fmd.ExecuteNonQuery();
        }


        public byte QualityIndicator;
        public int Satelites;
        public double HDOP;
        public double Altitude;
        public double AgeDGPS;
        public float SpeedKnots;
        public double IMUHeading;
        public double IMURoll;


        public static void AddNMEAData()
        {
            if (_conn == null)
                throw new Exception("No database connection");
            if (UDPHandler.NMEAData == null)
                throw new Exception("No NMEA data");

            using SqliteCommand fmd = _conn.CreateCommand();
            fmd.CommandText = @"INSERT INTO GPSdata (time, latitude, longitude, altitude, QualityIndicator, Satelites, hdop, age, speed, IMUHeading, IMURoll) VALUES (@time, @lat, @lon, @alt, @quality, @sats, @hdop, @age, @speed, @heading, @roll)";
            fmd.Parameters.AddWithValue("@time", new DateTimeOffset(UDPHandler.NMEAData.ReciveSystemTime).ToUnixTimeMilliseconds());
            fmd.Parameters.AddWithValue("@lat", UDPHandler.NMEAData.Latitude);
            fmd.Parameters.AddWithValue("@lon", UDPHandler.NMEAData.Longitude);
            fmd.Parameters.AddWithValue("@alt", UDPHandler.NMEAData.Altitude);
            fmd.Parameters.AddWithValue("@quality", (int)UDPHandler.NMEAData.QualityIndicator);
            fmd.Parameters.AddWithValue("@sats", UDPHandler.NMEAData.Satelites);
            fmd.Parameters.AddWithValue("@hdop", UDPHandler.NMEAData.HDOP);
            fmd.Parameters.AddWithValue("@age", UDPHandler.NMEAData.AgeDGPS);
            fmd.Parameters.AddWithValue("@speed", UDPHandler.NMEAData.SpeedKnots);
            fmd.Parameters.AddWithValue("@heading", UDPHandler.NMEAData.IMUHeading);
            fmd.Parameters.AddWithValue("@roll", UDPHandler.NMEAData.IMURoll);
            fmd.CommandType = CommandType.Text;
            fmd.ExecuteNonQuery();
        }
    }
}
