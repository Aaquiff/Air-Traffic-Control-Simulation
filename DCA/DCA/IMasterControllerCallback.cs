using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;


namespace DCA
{
    [ServiceContract]
    public interface IMasterControllerCallback
    {
        [OperationContract]
        Airport Simulate();

        [OperationContract(IsOneWay =true)]
        void HandoverPlane(AirPlane plane);
    }
}
