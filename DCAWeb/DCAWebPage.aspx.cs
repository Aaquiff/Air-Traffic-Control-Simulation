using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DCA;
using System.ServiceModel;
using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page
{
    IMasterController m_DCA;
    List<Airport> airports;

    public _Default()
    {
        m_DCA = CreateChannel();
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        airports = m_DCA.GetAirports();
        LoadAirports(airports);
        //LoadAirPlanes(airports.First());
    }

    public void LoadAirPlanes(Airport airport)
    {
        string html = "<table class='table'> ";
        html += "<thead>";
        html += "<th>Plane Id</th>";
        html += "<th>State</th>";
        html += "<th>Type</th>";
        html += "<th>Remaining Fuel</th>";
        html += "</thead>";
        html += "<tbody>";
        foreach (AirPlane plane in airport.LandedPlanes)
        {
            html += "<tr>";
            html += "<td>" + plane.AirplaneId + "</td>";
            html += "<td>" + plane.State + "</td>";
            html += "<td>" + plane.PlaneType + "</td>";
            html += "<td>" + plane.Fuel + "</td>";
            html += "</tr>";
        }
        html += "</tbody>";
        html += "</table>";
        divAirplanes.InnerHtml = html;
    }

    public void LoadAirports(List<Airport> airports)
    {
        string html = "<h3>Airports</h3>  <div class='list - group'>";
        foreach (Airport airport in airports)
        {
            html += "<a class='list-group-item' href='#' onclick='LoadPlanes("+airport.AirportId+");' >" + airport.Name + "</a>";
        }
        html += "</div>";
        divAirports.InnerHtml = html;
    }

    public IMasterController CreateChannel()
    {
        NetTcpBinding tcpBinding = new NetTcpBinding();
        tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
        tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
        DuplexChannelFactory<IMasterController> channelFactory = new DuplexChannelFactory<IMasterController>(new DummyCallBack(), tcpBinding, "net.tcp://localhost:50002/DCAMaster");
        return channelFactory.CreateChannel();
    }


    protected void btnStep_ServerClick(object sender, EventArgs e)
    {
        foreach (Airport item in airports)
        {
            LoadAirPlanes(airports.Last());
        }
    }
}


public class DummyCallBack : IMasterControllerCallback
{
    public void HandoverPlane(AirPlane plane)
    {
        throw new NotImplementedException();
    }

    public Airport Simulate()
    {
        throw new NotImplementedException();
    }
}