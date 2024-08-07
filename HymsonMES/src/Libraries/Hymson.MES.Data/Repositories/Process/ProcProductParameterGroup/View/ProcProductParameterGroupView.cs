using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process.View
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcProductParameterGroupView : BaseEntity
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 工序参数
    /// </summary>
    public class ProcedureParamView
    {
        /// <summary>  
        /// 参数组
        /// </summary>  
        public string Code { get; set; }

        /// <summary>  
        /// 参数组名称
        /// </summary>  
        public string Name { get; set; }

        /// <summary>  
        /// 参数组版本 
        /// </summary>  
        public string Version { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>  
        /// 上限值，可能来自t2表  
        /// </summary>  
        public decimal? UpperLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 中心值，可能来自t2表  
        /// </summary>  
        public decimal? CenterValue { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 下限值，可能来自t2表  
        /// </summary>  
        public decimal? LowerLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 工序代码，可能来自t3表  
        /// </summary>  
        public string ProcedureCode { get; set; }

        /// <summary>  
        /// 工序名称，可能来自t3表  
        /// </summary>  
        public string ProcedureName { get; set; }

        /// <summary>  
        /// 参数名称，可能来自t4表  
        /// </summary>  
        public string ParameterName { get; set; }

        /// <summary>  
        /// 参数代码，可能来自t4表  
        /// </summary>  
        public string ParameterCode { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string ParameterUnit { get; set; }
    }


}
