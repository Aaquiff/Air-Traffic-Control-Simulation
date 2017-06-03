using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.CompilerServices;

namespace DCA
{
    [ServiceContract(CallbackContract = typeof(IMasterControllerCallback))]
    public interface IMasterController
    {
        /* Slave */
        [OperationContract]
        Airport AttachSlave();

        [OperationContract(IsOneWay = true)]
        void Detach();

        [OperationContract]
        void UpdateMaster(Airport airport);

        [OperationContract(IsOneWay =true)]
        void Handover(AirPlane plane);

        [OperationContract]
        List<Airport> StepAsync();

        [OperationContract]
        List<Airport> GetAirports();
    }
}
