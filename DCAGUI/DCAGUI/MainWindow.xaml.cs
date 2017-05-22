using DCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DCAGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IMasterController channel;
        List<Airport> airports;

        public List<Airport> Airports { get => airports; set => airports = value; }

        public MainWindow()
        {
            InitializeComponent();
            channel = CreateChannel();

            LoadAirports();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (channel != null)
            {
                channel.init();
                LoadAirports();
            }
        }

        public IMasterController CreateChannel()
        {
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            CallBackObject callbackObj = new CallBackObject();
            DuplexChannelFactory<IMasterController> channelFactory = new DuplexChannelFactory<IMasterController>(callbackObj, tcpBinding, Constants._SURL);
            return channelFactory.CreateChannel();
        }

        public void LoadAirports()
        {
            if (channel != null)
            {
                Airports = channel.GetAirports();
                airportListView.ItemsSource = Airports;
                
            }
        }

        private void airportListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshPlaneList();
        }

        private void RefreshPlaneList()
        {
            if (airportListView.SelectedItem != null)
            {
                Airport airport = (Airport)airportListView.SelectedItem;
                if (airport != null)
                {
                    List<AirPlane> planes = airport.LandedPlanes.ToList();
                    planesListView.ItemsSource = planes;
                    planesListView.Items.Refresh();

                    outBoundPlanesListView.ItemsSource = airport.InTransit;
                    outBoundPlanesListView.Items.Refresh();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadAirports();
            RefreshPlaneList();
        }
    }

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CallBackObject : IMasterControllerCallback
    {
        [OperationBehavior]
        public void Simulate()
        {
            Console.WriteLine(" GUI Callback");
        }
    }
}
