using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SizeEvaluationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SizeEvaluationService.svc or SizeEvaluationService.svc.cs at the Solution Explorer and start debugging.
    public class SizeEvaluationService : ISizeEvaluationService
    {
        public string Evaluate(long count)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (count == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(count);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return (Math.Sign(count) * num).ToString() + suf[place];
        }
    }
}
