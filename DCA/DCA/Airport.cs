using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ATCDatabase;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace DCA
{
    [DataContract]
    public class Airport
    {
        [DataMember]
        int airportId;

        [DataMember]
        Queue<AirRoute> departingRoutes;
        
        [DataMember]
        string name;

        [DataMember]
        Queue<AirPlane> landedPlanes;

        [DataMember]
        List<AirPlane> inTransit;

        [DataMember]
        List<AirPlane> circlingPlanes;

        [DataMember]
        List<AirPlane> enteringCirclingPlanes;

        [DataMember]
        List<AirPlane> crashedPlanes;

        public Airport(int aId)
        {
            airportId = aId;
            ATCDB db = new ATCDB();
            db.LoadAirport(aId, out name);
            int[] pids = db.GetAirplaneIDsForAirport(airportId);
            inTransit = new List<AirPlane>();
            landedPlanes = new Queue<AirPlane>();
            enteringCirclingPlanes = new List<AirPlane>();
            circlingPlanes = new List<AirPlane>();
            crashedPlanes = new List<AirPlane>();
            foreach (int item in pids)
            {
                AirPlane plane = new AirPlane(item);
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
            get
            {
                return LandedPlanes.ToList().Concat(CirclingPlanes).Concat(EnteringCirclingPlanes).Concat(CrashedPlanes).Concat(InTransit).ToList();
            }
        }

        public Queue<AirPlane> LandedPlanes
        {
            get
            {
                return landedPlanes;
            }
            set
            {
                landedPlanes = value;
            }
        }
        public List<AirPlane> CirclingPlanes
        {
            get
            {
                return circlingPlanes;
            }
            set { circlingPlanes = value;
            }
        }

        public List<AirPlane> EnteringCirclingPlanes
        {
            get
            {
                return enteringCirclingPlanes;
            }
            set
            {
                enteringCirclingPlanes = value;
            }

        }

        public Queue<AirRoute> DepartingRoutes
        {
            get { return departingRoutes; }
            set { departingRoutes = value; }
        }
        public List<AirPlane> InTransit
        {
            get
            {
                return inTransit;
            }
            set { inTransit = value; }
            }
        public List<AirPlane> CrashedPlanes
        {
            get
            {
                return crashedPlanes;
            }
            set { crashedPlanes = value; }
            }
    }
}
