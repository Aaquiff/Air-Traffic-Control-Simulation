using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATCDatabase;
using System.Runtime.Serialization;

namespace DCA
{
    [DataContract]
    public class Airport
    {
        [DataMember]
        int airportId;

        [DataMember]
        List<AirPlane> planes;

        [DataMember]
        Queue<AirRoute> departingRoutes;
        
        [DataMember]
        string name;

        [DataMember]
        Queue<AirPlane> landedPlanes;

        [DataMember]
        List<AirPlane> inTransit;

        [DataMember]
        Queue<AirPlane> circlingPlanes;

        [DataMember]
        Queue<AirPlane> enteringCirclingPlanes;

        public Airport(int aId)
        {
            airportId = aId;
            ATCDB db = new ATCDB();
            db.LoadAirport(aId, out name);
            int[] pids = db.GetAirplaneIDsForAirport(airportId);
            planes = new List<AirPlane>();
            inTransit = new List<AirPlane>();
            landedPlanes = new Queue<AirPlane>();
            foreach (int item in pids)
            {
                AirPlane plane = new AirPlane(item);
                planes.Add(plane);
                landedPlanes.Enqueue(plane);
            }

            int[] arids = db.GetDepartingAirRouteIDsForAirport(airportId);
            DepartingRoutes = new Queue<AirRoute>();
            for (int i = 0; i < arids.Length; i++)
            {
                AirRoute route = new AirRoute(arids[i]);
                departingRoutes.Enqueue(route);
            }
        }

        
        public int AirportId
        {
            get { return airportId; }
        }
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        public List<AirPlane> Planes
        {
            get { return planes; }
            set { planes = value; }
        }

        public Queue<AirPlane> LandedPlanes { get => landedPlanes; set => landedPlanes = value; }
        public Queue<AirPlane> CirclingPlanes { get => circlingPlanes; set => circlingPlanes = value; }
        public Queue<AirPlane> EnteringCirclingPlanes { get => enteringCirclingPlanes; set => enteringCirclingPlanes = value; }
        public Queue<AirRoute> DepartingRoutes { get => departingRoutes; set => departingRoutes = value; }
        public List<AirPlane> InTransit { get => inTransit; set => inTransit = value; }
    }
}
