/*
 *creator: Karl
 *
 *describe: 物料维护    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
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
    /// @date 2023-02-07 11:16:51
    /// </summary>
    public class ProcMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 描述 :所属物料组ID 
        /// 空值 : false  
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 描述 :来源 
        /// 空值 : true  
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述 :采购类型 
        /// 空值 : true  
        /// </summary>
        public string BuyType { get; set; }

        /// <summary>
        /// 描述 :ID (工艺路线)
        /// 空值 : true  
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 描述 :ID （工序Bom）
        /// 空值 : true  
        /// </summary>
        public long? ProcedureBomId { get; set; }

        /// <summary>
        /// 描述 :批次大小 
        /// 空值 : true  
        /// </summary>
        public int? Batch { get; set; }

        /// <summary>
        /// 描述 :计量单位(字典定义) 
        /// 空值 : true  
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 描述 :内/外序列号 
        /// 空值 : true  
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 描述 :验证掩码组 
        /// 空值 : true  
        /// </summary>
        public string ValidationMaskGroup { get; set; }

        /// <summary>
        /// 描述 :基于时间(字典定义) 
        /// 空值 : true  
        /// </summary>
        public string BaseTime { get; set; }

        /// <summary>
        /// 描述 :消耗公差 
        /// 空值 : true  
        /// </summary>
        public string ConsumptionTolerance { get; set; }

        /// <summary>
        /// 描述 :是否默认版本 
        /// 空值 : 0  
        /// </summary>
        public bool IsDefaultVersion { get; set; }
    }
}
