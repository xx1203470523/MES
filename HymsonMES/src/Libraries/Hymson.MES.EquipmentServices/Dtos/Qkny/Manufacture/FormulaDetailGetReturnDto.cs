using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 配方明细返回
    /// </summary>
    public record FormulaDetailGetReturnDto 
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<FormulaParamList> ParamList { get; set; } = new List<FormulaParamList>();
    }

    /// <summary>
    /// 配方参数
    /// </summary>
    public record FormulaParamList
    {
        /// <summary>  
        /// 操作顺序  
        /// </summary>  
        public int SepOrder { get; set; }

        /// <summary>  
        /// 操作类型  
        /// </summary>  
        public string Category { get; set; } = "";

        /// <summary>  
        /// 物料编码  
        /// </summary>  
        public string MarterialCode { get; set; } = "";

        /// <summary>  
        /// 物料组编码  
        /// </summary>  
        public string MarerialGroupCode { get; set; } = "";

        /// <summary>  
        /// 参数编码  
        /// </summary>  
        public string ParameCode { get; set; } = "";

        /// <summary>  
        /// 参数值  
        /// </summary>  
        public string ParamValue { get; set; } = "";

        /// <summary>  
        /// 功能码  
        /// </summary>  
        public string FunctionCode { get; set; } = "";

        /// <summary>  
        /// 单位  
        /// </summary>  
        public string Unit { get; set; } = "";
    }

}
