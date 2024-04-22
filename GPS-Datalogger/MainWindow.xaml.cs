using System.Globalization;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPS_Datalogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            UDPHandler.StartListen();

            //SetTimer();
            UDPHandler.NEMAUpdated += UpdateUI;
            UDPHandler.NEMAUpdated += DataLogger.AddNMEAData;
            DataLogger.Init();
            
            InitializeComponent();
        }

        private List<double> ChartData = new List<double>();
        private void UpdateUI()
        {
            if (UDPHandler.NMEAData != null)
            {
                ChartData.Add(UDPHandler.NMEAData.Altitude);
                while (ChartData.Count > 600)
                    ChartData.RemoveAt(0);

                this.Dispatcher.Invoke(() =>
                {
                    SystemTime.Text = UDPHandler.NMEAData.ReciveSystemTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    Latitude.Text = UDPHandler.NMEAData.Latitude.ToString();
                    Longitude.Text = UDPHandler.NMEAData.Longitude.ToString();
                    QualityIndicator.Text = UDPHandler.NMEAData.QualityIndicator.ToString();
                    Satelites.Text = UDPHandler.NMEAData.Satelites.ToString();
                    Altitude.Text = UDPHandler.NMEAData.Altitude.ToString();
                    SpeedKnots.Text = UDPHandler.NMEAData.SpeedKnots.ToString();
                    IMUHeading.Text = UDPHandler.NMEAData.IMUHeading.ToString();
                    IMURoll.Text = UDPHandler.NMEAData.IMURoll.ToString();

                    Double[] dataY = ChartData.ToArray();
                    int[] dataX = Enumerable.Range(1, dataY.Length).ToArray();

                    LineChart.Plot.Clear();
                    LineChart.Plot.Add.Scatter(dataX, dataY);
                    LineChart.Plot.Axes.AutoScaleX();
                    LineChart.Plot.Axes.AutoScaleY();
                    LineChart.Refresh();


                });
            }
        }

    }
}