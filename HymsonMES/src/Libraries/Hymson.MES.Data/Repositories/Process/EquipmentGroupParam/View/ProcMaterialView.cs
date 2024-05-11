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

    /// <summary>  
    /// 表示主查询结果的实体类  
    /// </summary>  
    public class ProcEquipmentGroupParamDetailView
    {
        /// <summary>  
        /// 代码  
        /// </summary>  
        public string Code { get; set; }

        /// <summary>  
        /// 版本  
        /// </summary>  
        public string Version { get; set; }

        /// <summary>  
        /// 更新时间  
        /// </summary>  
        public DateTime CreatedOn { get; set; }

        /// <summary>  
        /// 更新时间  
        /// </summary>  
        public DateTime ?UpdatedOn { get; set; }

        /// <summary>  
        /// 最大值  
        /// </summary>  
        public decimal? MaxValue { get; set; } // 假设是decimal类型，根据实际情况可以修改  

        /// <summary>  
        /// 最小值  
        /// </summary>  
        public decimal? MinValue { get; set; } // 假设是decimal类型，根据实际情况可以修改  

        /// <summary>
        /// 设定值
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>  
        /// 参数ID  
        /// </summary>  
        public long ParamId { get; set; }

        /// <summary>  
        /// 参数代码  
        /// </summary>  
        public string ParameterCode { get; set; }

        /// <summary>  
        /// 参数名称  
        /// </summary>  
        public string ParameterName { get; set; }
    }


    #endregion

}
