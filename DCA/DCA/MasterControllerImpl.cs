using ATCDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DCA
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single)]
    internal class MasterControllerImpl : IMasterController
    {
        private delegate void StepAsyncDelegate(int i);
        private AsyncCallback callBackDelegate;
        //Callback interface to each ATC slave
        IMasterControllerCallback[] callBacks;
        //List of airports
        Airport[] airports;
        //Number of clients connected
        int count;

        ~MasterControllerImpl()
        {
            Console.WriteLine("Client Disconnected");
        }
        
        public MasterControllerImpl()
        {
            count = -1;
            ATCDB atcdb = new ATCDB();
            int[] aids = atcdb.GetAirportIDList();
            callBacks = new IMasterControllerCallback[aids.Length];
            airports = new Airport[aids.Length];
            for (int i = 0; i < aids.Length; i++)
            {
                airports[i] = new Airport(aids[i]);
            }
        }

        [OperationBehavior]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Airport Attach()
        {
            if (count > 2)
            {
                Console.WriteLine("Max number of clients reached!");
                return null;
            }
            Console.WriteLine("Assigning airport "+airports[count+1].Name + " to client");
            IMasterControllerCallback cb = OperationContext.Current.GetCallbackChannel<IMasterControllerCallback>();
            count++;
            callBacks[count] = cb;
            return airports[count];
        }

        [OperationBehavior]
        public void Detach()
        {
            Console.WriteLine("Detaching Slave");
        }

        [OperationBehavior]
        public void init()
        {
            for (int i = 0; i < callBacks.Length; i++)
            {
                callBacks[i].Simulate();
            }
        }

        public void StepAsync()
        {
            IAsyncResult[] result = new IAsyncResult[callBacks.Length];
            StepAsyncDelegate sad = new StepAsyncDelegate(StepAsyncInvoke);
            callBackDelegate = StepAsync_OnComplete;
            for (int i = 0; i < callBacks.Length; i++)
            {
                result[i] = sad.BeginInvoke(i,callBackDelegate,null);
                sad.EndInvoke(result[i]);
            }
        }

        public void StepAsyncInvoke(int index)
        {
            callBacks[index].Simulate();
        }

        public void StepAsync_OnComplete(IAsyncResult result)
        {
            Console.WriteLine("StepAsync_OnComplete");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(Airport airport)
        {
            Console.WriteLine("Slaves Updating Master");
            Console.WriteLine(airport.Name);
            foreach (Airport item in airports)

            for (int i = 0; i < airports.Length; i++)
            {
                    if (airports[i].AirportId == airport.AirportId)
                        airports[i] = airport;
            }
        }

        public List<Airport> GetAirports()
        {
            List<Airport> airportList = new List<Airport>();
            foreach (Airport item in airports)
            {
                airportList.Add(item);
            }
            return airportList;
        }

        //Handover an airplane from one airport to another
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Handover(AirPlane plane)
        {
            int toAiportID = plane.CurrentRoute.ToAirportID;
            for (int i = 0; i < airports.Length; i++)
            {
                if (airports[i].AirportId.Equals(toAiportID) )
                {
                    callBacks[i].HandoverPlane(plane);
                    break;
                }
            }
        }
    }
}
