/*
 *creator: Karl
 *
 *describe: 设备参数组 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组 分页参数
    /// </summary>
    public class ProcEquipmentGroupParamPagedQuery : PagerInfo
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 参数组类型;1、开机参数 2、设备过程参数
        /// </summary>
        public EquipmentGroupParamTypeEnum? Type { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
