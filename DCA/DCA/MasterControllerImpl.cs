using ATCDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;


namespace DCA
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = true, InstanceContextMode = InstanceContextMode.Single)]
    internal class MasterControllerImpl : IMasterController
    {
        private delegate void StepAsyncDelegate(int i);
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
        public Airport AttachSlave()
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
        public List<Airport> StepAsync()
        {
            Console.WriteLine("Stepping");
            IAsyncResult[] result = new IAsyncResult[callBacks.Length];
            StepAsyncDelegate[] stepAsyncDelegates = new StepAsyncDelegate[callBacks.Length];
            //StepAsyncDelegate sad = new StepAsyncDelegate(StepAsyncInvoke);
            
            for (int i = 0; i < callBacks.Length; i++)
            {
                AsyncCallback callBackDelegate;
                callBackDelegate = StepAsync_OnComplete;
                stepAsyncDelegates[i] = new StepAsyncDelegate(StepAsyncInvoke);

                result[i] = stepAsyncDelegates[i].BeginInvoke(i,null,null);
            }
            //Wait for all async calls to finish
            for (int i = 0; i < callBacks.Length; i++)
            {

                //stepAsyncDelegates[i].EndInvoke(result[i]);
            }
            return GetAirports();
        }

        public void StepAsyncInvoke(int index)
        {
            airports[index] = callBacks[index].Simulate();
        }

        public void StepAsync_OnComplete(IAsyncResult result)
        {
            Console.WriteLine("StepAsync_OnComplete");
            StepAsyncDelegate stepAsyncDelg;
            AsyncResult asnycObj = (AsyncResult)result;

            if(asnycObj.EndInvokeCalled == false)
            {
                stepAsyncDelg = (StepAsyncDelegate)asnycObj.AsyncDelegate;

            }
        }

        [OperationBehavior]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateMaster(Airport airport)
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

        [OperationBehavior]
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
        [OperationBehavior]
        public void Handover(AirPlane plane)
        {
            Console.WriteLine("Master receiving plane for handover");
            int toAiportID = plane.CurrentRoute.ToAirportID;
            for (int i = 0; i < airports.Length; i++)
            {
                if (airports[i].AirportId.Equals(toAiportID))
                {
                    callBacks[i].HandoverPlane(plane);
                    break;
                }
            }
        }

        public List<AirPlane> GetAirplanes(int airportId)
        {
            foreach (Airport item in airports)
            {
                if (item.AirportId.Equals(airportId))
                    return item.LandedPlanes.ToList();
            }
            Console.WriteLine("Airport Not Found");
            throw new Exception("Airport Not Found");
        }

    }
}
