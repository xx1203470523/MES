using Hymson.MES.BackgroundServices.CoreServices.Model.Rotor;
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
                return WorkPosNo.Split('.')[0];
            }
        }
    }
}
