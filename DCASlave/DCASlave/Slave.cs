using DCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DCASlave
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class Slave : IMasterControllerCallback
    {
        public Airport airport;
        IMasterController m_Master;

        public Slave()
        {
            m_Master = CreateChannel();
            airport = m_Master.Attach();
            Console.WriteLine(airport.Name);
        }

        ~Slave()
        {
            m_Master.Detach();
        }

        //Creates a channel and return a IMasterControllerObject
        public IMasterController CreateChannel()
        {
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            DuplexChannelFactory<IMasterController> channelFactory = new DuplexChannelFactory<IMasterController>(this, tcpBinding, Constants._SURL);
            return channelFactory.CreateChannel();
        }

        public void Simulate()
        {
            Console.WriteLine("Callback to airport : " + this.airport.Name);
            foreach (AirPlane item in airport.LandedPlanes)
            {
                item.TimeWaited += 15;
            }
            //Landed plane should take off
            if (airport.LandedPlanes.Count > 0)
            {
                if (airport.LandedPlanes.Peek().TimeWaited >= 60)
                {
                    //Get first plane to leave from the landed queue
                    AirPlane airPlane = airport.LandedPlanes.Dequeue();
                    //Set Time waited to -1 since it's in flight
                    airPlane.TimeWaited = -1;
                    //Set the plane state to INTRANSIT
                    airPlane.State = AirPlane.AirPlaneState.INTRANSIT;
                    //Current airport ID is -1 because plane is INTRANSIT
                    airPlane.CurrentAirportID = -1;
                    //Get the next route from the airport
                    AirRoute route = airport.DepartingRoutes.Dequeue();
                    //Set that route as the current route for the plane
                    airPlane.CurrentRoute = route;
                    //Put the route back into the end queue so it can be used again
                    airport.DepartingRoutes.Enqueue(route);
                    //Add it to intransit planes
                    airport.InTransit.Add(airPlane);

                    //Fuel up for the route
                    airPlane.Fuel = (airPlane.CurrentRoute.DistanceKm / airPlane.CruisingKPH) * airPlane.FuelConsPerHour * 1.15;
                }
            }
            //Planes in transit should move with time
            if (airport.InTransit.Count > 0)
            {
                foreach (AirPlane plane in airport.InTransit)
                {
                    //Increase distance travelled 15 mins
                    plane.DistanceTravelled += plane.CruisingKPH * 1 / 4;
                    //Decrease Fuel for 15 mins
                    plane.Fuel -= plane.FuelConsPerHour * 1 / 4;

                    //If plane is within 300Km to the destination airport
                    if (plane.CurrentRoute.DistanceKm - plane.DistanceTravelled < 300)
                    {
                        plane.State = AirPlane.AirPlaneState.ENTERINGCIRCLE;
                    }
                }
            }

            m_Master.Update(airport);
        }


    }
}
