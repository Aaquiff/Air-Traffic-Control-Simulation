using ATCDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DCA
{

    [DataContract]
    public class AirPlane
    {
        public enum AirPlaneState
        {
            LANDED,
            INTRANSIT,
            ENTERINGCIRCLE,
            CIRCLING,
            CRASHED
        }

        [DataMember]
        int airplaneId;
        [DataMember]
        string planeType;
        [DataMember]
        double cruisingKPH;
        [DataMember]
        double fuelConsPerHour;
        [DataMember]
        int currentAirportID;
        [DataMember]
        AirPlaneState state;
        [DataMember]
        AirRoute currentRoute;
        [DataMember]
        double distanceTravelled;
        [DataMember]
        double timeWaited;
        [DataMember]
        double fuel;

        public AirPlane(int aid)
        {
            AirplaneId = aid;
            ATCDB actdb = new ATCDB();
            actdb.LoadAirplane(AirplaneId, out planeType, out cruisingKPH, out fuelConsPerHour, out currentAirportID);
            state = AirPlaneState.LANDED;
            distanceTravelled = 0;
            fuel = 0;
            timeWaited = 0;
        }

        public int AirplaneId
        {
            get => airplaneId;
            set => airplaneId = value;
        }
        public string PlaneType
        {
            get => planeType;
            set => planeType = value;
        }
        public double CruisingKPH
        {
            get => cruisingKPH;
            set => cruisingKPH = value;
        }
        public double FuelConsPerHour
        {
            get => fuelConsPerHour;
            set => fuelConsPerHour = value;
        }
        public int CurrentAirportID
        {
            get => currentAirportID;
            set => currentAirportID = value;
        }
        public AirRoute CurrentRoute {
            get => currentRoute;
            set => currentRoute = value;
        }
        public Double DistanceTravelled{
            get => distanceTravelled;
            set => distanceTravelled = value;
        }
        public AirPlaneState State {
            get => state;
            set => state = value;
        }
        public Double TimeWaited {
            get => timeWaited;
            set => timeWaited = value;
        }
        public double Fuel {
            get => fuel;
            set => fuel = value;
        }
    }
}
