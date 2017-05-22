using ATCDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DCA
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single)]
    internal class MasterControllerImpl : IMasterController
    {
        IMasterControllerCallback[] callBacks;
        Airport[] airports;
        int count = -1;

        ~MasterControllerImpl()
        {
            Console.WriteLine("Client Disconnected");
        }
        
        public MasterControllerImpl()
        {
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
    }
}
