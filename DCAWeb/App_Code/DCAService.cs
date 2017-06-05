using DCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DCAService" in code, svc and config file together.
public class DCAService : IDCAService, IMasterControllerCallback
{
    IMasterController m_DCA;
    public DCAService()
    {
        m_DCA = CreateChannel();
    }

    public IMasterController CreateChannel()
    {
        NetTcpBinding tcpBinding = new NetTcpBinding();
        tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
        tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
        DuplexChannelFactory<IMasterController> channelFactory = new DuplexChannelFactory<IMasterController>(this, tcpBinding, "net.tcp://localhost:50002/DCAMaster");
        return channelFactory.CreateChannel();
    }

    public List<Airport> GetAirports()
    {
        return m_DCA.GetAirports();
    }

    public List<AirPlane> GetAirplanes(int airportId)
    {
        List<Airport> airports = GetAirports();
        foreach (Airport item in airports)
        {
            if (item.AirportId.Equals(airportId))
                return item.Planes.ToList();
        }
        return null;
    }

    public List<Airport> StepAsync()
    {
        return m_DCA.StepAsync();
    }


    public void HandoverPlane(AirPlane plane)
    {
        throw new NotImplementedException();
    }

    public Airport Simulate()
    {
        throw new NotImplementedException();
    }

    
}
