using ATCDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace DCA
{
    [DataContract]
    public class AirRoute
    {
        [DataMember]
        int routeId;
        [DataMember]
        double distanceKm;
        [DataMember]
        int toAirportID;
        [DataMember]
        int fromAirportID;

        public AirRoute(int prouteId)
        {
            ATCDB db = new ATCDB();
            RouteId = prouteId;
            db.LoadAirRoute(routeId, out fromAirportID, out toAirportID, out distanceKm);
        }

        public int ToAirportID
        {
            get { return toAirportID; }
            set
            {
                toAirportID = value;
            }
        }
        public int RouteId
        {
            get
            { return routeId; }
            set
            {
                routeId = value;
            }
        }
        public double DistanceKm
        {
            get
            { return distanceKm; }
            set
            {
                distanceKm = value;
            }
        }
        public int FromAirportID
        {
            get
            { return fromAirportID; }
            set
            { fromAirportID = value; }
        }
    }
}
