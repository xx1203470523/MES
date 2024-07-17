using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param.View
{
    /// <summary>
    /// 马威参数
    /// </summary>
    public class ProcedureParamMavelView
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
        public int DataType { get; set; }
    }
}
