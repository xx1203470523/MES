using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.View
{
    public  class ProcFormulaView: ProcFormulaEntity
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName {  get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set;}

    }

    /// <summary>
    /// 配方列表
    /// </summary>
    public class ProcFormulaListViewDto
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 产品
        /// </summary>
        public string MaterialCode { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 获取配方详情
    /// </summary>
    public class ProcFormulaDetailViewDto
    {
        /// <summary>  
        /// 站点ID  
        /// </summary>  
        public long SiteId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Serial { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>  
        /// 编码  
        /// </summary>  
        public string FormulaCode { get; set; }

        /// <summary>  
        /// 版本  
        /// </summary>  
        public string Version { get; set; }

        /// <summary>  
        /// 功能编码  
        /// </summary>  
        public string FunctionCode { get; set; }

        /// <summary>  
        /// 设置值  
        /// </summary>  
        public string Setvalue { get; set; }

        /// <summary>  
        /// 操作码
        /// </summary>  
        public string OperationCode { get; set; }

        /// <summary>  
        /// 类型  
        /// </summary>  
        public string Type { get; set; }

        /// <summary>  
        /// 材料编码  
        /// </summary>  
        public string MaterialCode { get; set; }

        /// <summary>  
        /// 材料组编码  
        /// </summary>  
        public string MaterialGroupCode { get; set; }

        /// <summary>  
        /// 参数编码  
        /// </summary>  
        public string ParameterCode { get; set; }

        /// <summary>  
        /// 名称  
        /// </summary>  
        public string FunctionName { get; set; }
    }

}
