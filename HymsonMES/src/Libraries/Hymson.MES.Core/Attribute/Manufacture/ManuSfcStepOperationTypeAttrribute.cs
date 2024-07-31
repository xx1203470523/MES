using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Attribute.Manufacture
{
    [AttributeUsage(AttributeTargets.All)]
    public class ManuSfcStepOperationTypeAttrribute : System.Attribute
    {
        /// <summary>
        /// 操作对应的程序或者作业
        /// </summary>
        /// <param name="jobOrAssemblyCode"></param>
        /// <param name="jobOrAssemblyName"></param>
        public ManuSfcStepOperationTypeAttrribute(string jobOrAssemblyCode, JobOrAssemblyNameEnum jobOrAssemblyName)
        {
            JobOrAssemblyCode = jobOrAssemblyCode;
            JobOrAssemblyName = jobOrAssemblyName;
        }
        /// <summary>
        /// 作业编码
        /// </summary>
        public string JobOrAssemblyCode { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public JobOrAssemblyNameEnum JobOrAssemblyName { get; set; }
    }
}
