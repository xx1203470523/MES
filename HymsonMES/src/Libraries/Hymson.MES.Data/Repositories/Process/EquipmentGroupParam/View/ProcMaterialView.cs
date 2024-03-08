using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcEquipmentGroupParamView : ProcEquipmentGroupParamEntity
    {
        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string MaterialVersion { get; set; }

        public string ProcedureCode { get; set; }

        public string ProcedureName { get; set; }
    }

    #region 顷刻

    /// <summary>
    /// 根据设备ID和产品型号查询
    /// 配方编码，版本，产品编码，最后更新时间
    /// </summary>
    public class ProcEquipmentGroupParamEquProductView : ProcEquipmentGroupParamEntity
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }
    }

    #endregion

}
