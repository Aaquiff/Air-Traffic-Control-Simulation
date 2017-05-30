using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DCA
{
    [ServiceContract(CallbackContract = typeof(IMasterControllerCallback))]
    public interface IMasterController
    {
        /* Slave */
        [OperationContract]
        Airport Attach();

        [OperationContract]
        void Detach();

        [OperationContract]
        void Update(Airport airport);

        [OperationContract]
        void Handover(AirPlane plane);

        /* GUI */
        [OperationContract]
        void init();

        [OperationContract]
        void StepAsync();

        [OperationContract]
        List<Airport> GetAirports();
    }
}
