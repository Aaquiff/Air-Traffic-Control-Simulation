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

        public List<Airport> Airports
        {
            get { return airports; }
            set { airports = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            channel = CreateChannel();

            LoadAirports();
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            if (channel != null)
            {
                channel.StepAsync();
                LoadAirports();
            }
        }
        //Creates a channel to the Master server
        public IMasterController CreateChannel()
        {
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            CallBackObject callbackObj = new CallBackObject();
            DuplexChannelFactory<IMasterController> channelFactory = new DuplexChannelFactory<IMasterController>(callbackObj, tcpBinding, "net.tcp://localhost:50002/DCAMaster");
            return channelFactory.CreateChannel();
        }
        //Load all airports from Master server
        public void LoadAirports()
        {
            if (channel != null)
            {
                Airports = channel.GetAirports();
                airportListView.ItemsSource = Airports;
                //RefreshPlaneList();
            }
        }
        
        private void airportListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshPlaneList();
        }
        //Refresh the list of planes according to the airport selected
        private void RefreshPlaneList()
        {
            if (airportListView.SelectedItem != null)
            {
                Airport airport = (Airport)airportListView.SelectedItem;
                if (airport != null)
                {
                    List<AirPlane> circlingPlanes = airport.CirclingPlanes.ToList();
                    List<AirPlane> enteringCirclingPlanes = airport.EnteringCirclingPlanes.ToList();
                    List<AirPlane> crashedPlanes = airport.CrashedPlanes.ToList();

                    inBoundPlanesListView.ItemsSource = circlingPlanes.Concat(enteringCirclingPlanes).Concat(crashedPlanes);
                    inBoundPlanesListView.Items.Refresh();

                    List<AirPlane> landedPlanes = airport.LandedPlanes.ToList();
                    List<AirPlane> inTransitPlanes = airport.InTransit.ToList();

                    outBoundPlanesListView.ItemsSource = landedPlanes.Concat(inTransitPlanes);
                    outBoundPlanesListView.Items.Refresh();
                }
            }
        }

        

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAirports();
            RefreshPlaneList();
        }

    }

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CallBackObject : IMasterControllerCallback
    {
        public void HandoverPlane(AirPlane plane)
        {
            throw new NotImplementedException();
        }

        [OperationBehavior]
        public void Simulate()
        {
            Console.WriteLine(" GUI Callback");
        }
    }
}
