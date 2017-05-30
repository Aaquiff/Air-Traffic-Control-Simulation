using DCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DCASlave
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = true)]
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
            DuplexChannelFactory<IMasterController> channelFactory = new DuplexChannelFactory<IMasterController>(this, tcpBinding, "net.tcp://localhost:50002/DCAMaster");
            return channelFactory.CreateChannel();
        }

        public void HandoverPlane(AirPlane plane)
        {
            airport.EnteringCirclingPlanes.Add(plane);
        }

        public void Simulate()
        {
            try
            {
                //Console.WriteLine("*******************************************");
                //Console.WriteLine("Callback to airport : " + this.airport.Name);
                foreach (AirPlane item in airport.LandedPlanes)
                {
                    item.TimeWaited += 15;
                }
                //Plane at the head of landed queue should leave if it has waited for 60 minutes
                if (airport.LandedPlanes.Count > 0 && airport.LandedPlanes.Peek().TimeWaited >= 60)
                {

                    //Get first plane to leave from the landed queue
                    AirPlane airPlane = airport.LandedPlanes.Dequeue();
                    Console.WriteLine("Plane " + airPlane.AirplaneId + " is leaving");
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

                //Planes in transit should move with time
                if (airport.InTransit.Count > 0)
                {
                    List<AirPlane> toRemove = new List<AirPlane>();
                    List<AirPlane> crashed = new List<AirPlane>();
                    foreach (AirPlane plane in airport.InTransit)
                    {
                        //Increase distance travelled 15 mins
                        plane.DistanceTravelled += plane.CruisingKPH * 1 / 4;
                        //Decrease Fuel for 15 mins
                        plane.Fuel -= plane.FuelConsPerHour * 1 / 4;

                        if (plane.Fuel < 0)
                            crashed.Add(plane);

                        //If plane is within 300Km to the destination airport
                        else if (plane.CurrentRoute.DistanceKm - plane.DistanceTravelled < 300)
                        {
                            plane.State = AirPlane.AirPlaneState.ENTERING_CIRCLING;
                            toRemove.Add(plane);
                            m_Master.Handover(plane);
                        }
                    }
                    foreach (AirPlane item in toRemove)
                        airport.InTransit.Remove(item);
                    foreach (AirPlane item in crashed)
                    {
                        airport.InTransit.Remove(item);
                        item.State = AirPlane.AirPlaneState.CRASHED;
                        airport.CrashedPlanes.Add(item);
                    }
                }
                
                //Planes Entering circling (within 300km) should move forward
                if (airport.EnteringCirclingPlanes.Count > 0)
                {
                    List<AirPlane> toRemove = new List<AirPlane>();
                    List<AirPlane> crashed = new List<AirPlane>();
                    foreach (AirPlane plane in airport.EnteringCirclingPlanes)
                    {
                        //Increase distance travelled 15 mins
                        plane.DistanceTravelled += plane.CruisingKPH * 1 / 4;
                        //Decrease Fuel for 15 mins
                        plane.Fuel -= plane.FuelConsPerHour * 1 / 4;
                        if (plane.Fuel < 0)
                            crashed.Add(plane);

                        //Plane has arrived. Remove from Entering circling and add to circling
                        if (plane.CurrentRoute.DistanceKm - plane.DistanceTravelled <= 0)
                        {
                            plane.State = AirPlane.AirPlaneState.CIRCLING;
                            //airport.EnteringCirclingPlanes.Remove(plane);
                            toRemove.Add(plane);
                            airport.CirclingPlanes.Add(plane);
                        }

                    }
                    foreach (AirPlane item in toRemove)
                    {
                        airport.EnteringCirclingPlanes.Remove(item);
                    }
                    foreach (AirPlane item in crashed)
                    {
                        airport.EnteringCirclingPlanes.Remove(item);
                        item.State = AirPlane.AirPlaneState.CRASHED;
                        airport.CrashedPlanes.Add(item);
                    }
                }
                
                if (airport.CirclingPlanes.Count > 0)
                {
                    //Assume the first plane in the list has the least fuel
                    AirPlane planeWithLowestFuel = airport.CirclingPlanes.First();
                    //Check if there are planes with lesser fuel than that and get it
                    foreach (AirPlane plane in airport.CirclingPlanes)
                    {
                        if (plane.Fuel < planeWithLowestFuel.Fuel)
                            planeWithLowestFuel = plane;
                    }
                    //Remove the selected plane from circling list and add to LandedQueue
                    airport.CirclingPlanes.Remove(planeWithLowestFuel);
                    planeWithLowestFuel.State = AirPlane.AirPlaneState.LANDED;
                    planeWithLowestFuel.DistanceTravelled = 0;
                    planeWithLowestFuel.CurrentAirportID =  airport.AirportId;
                    planeWithLowestFuel.TimeWaited = 0;
                    planeWithLowestFuel.CurrentRoute = null;

                    airport.LandedPlanes.Enqueue(planeWithLowestFuel);

                    List<AirPlane> crashed = new List<AirPlane>();
                    foreach (AirPlane plane in airport.CirclingPlanes)
                    {
                        //Decrease Fuel for 15 mins
                        plane.Fuel -= plane.FuelConsPerHour * 1 / 4;
                        if (plane.Fuel <= 0)
                            crashed.Add(plane);
                    }
                    //Remove the crashed planes
                    foreach (AirPlane item in crashed)
                    {
                        airport.CirclingPlanes.Remove(item);
                        item.State = AirPlane.AirPlaneState.CRASHED;
                        airport.CrashedPlanes.Add(item);
                    }
                }
                
                m_Master.Update(airport);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
