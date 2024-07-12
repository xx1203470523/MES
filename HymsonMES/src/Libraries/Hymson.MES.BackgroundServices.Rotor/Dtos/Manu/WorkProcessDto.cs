using Hymson.MES.BackgroundServices.Rotor.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Dtos.Manu
{
    /// <summary>
    /// 过站表
    /// </summary>
    public class WorkProcessDto : WorkProcessEntity
    {
        /// <summary>
        /// MES工序编码
        /// </summary>
        public string ProcedureCode 
        {
            get
            {
                var code = WorkPosNo.Split('.')[0];
                if(code.Length == 4)
                {
                    code = code.Substring(0,2) + "0" + code.Substring(2);
                }

                return code;
            }
        }
    }
}
