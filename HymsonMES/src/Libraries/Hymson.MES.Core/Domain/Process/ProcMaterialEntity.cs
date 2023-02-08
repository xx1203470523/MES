/*
 *creator: Karl
 *
 *describe: 物料维护    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料维护，数据实体对象   
    /// proc_material
    /// @author Karl
    /// @date 2023-02-08 04:47:44
    /// </summary>
    public class ProcMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; }

       /// <summary>
        /// 所属物料组ID
        /// </summary>
        public long GroupId { get; set; }

       /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

       /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

       /// <summary>
        /// 来源
        /// </summary>
        public string Origin { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

       /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 采购类型
        /// </summary>
        public string BuyType { get; set; }

       /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long? ProcessRouteId { get; set; }

       /// <summary>
        /// 工序BomID
        /// </summary>
        public long? ProcedureBomId { get; set; }

       /// <summary>
        /// 批次大小
        /// </summary>
        public int? Batch { get; set; }

       /// <summary>
        /// 计量单位(字典定义)
        /// </summary>
        public string Unit { get; set; }

       /// <summary>
        /// 内/外序列号
        /// </summary>
        public string SerialNumber { get; set; }

       /// <summary>
        /// 验证掩码组
        /// </summary>
        public string ValidationMaskGroup { get; set; }

       /// <summary>
        /// 基于时间(字典定义)
        /// </summary>
        public string BaseTime { get; set; }

       /// <summary>
        /// 消耗公差
        /// </summary>
        public string ConsumptionTolerance { get; set; }

       
    }
}
