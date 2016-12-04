using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISizeEvaluationService" in both code and config file together.
    [ServiceContract]
    public interface ISizeEvaluationService
    {
        [OperationContract]
        string Evaluate(long bytes);
    }
}
