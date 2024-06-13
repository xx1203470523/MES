/*
 *creator: Karl
 *
 *describe: 设备备件记录表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-06-12 10:29:55
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表，数据实体对象   
    /// equ_sparepart_record
    /// @author pengxin
    /// @date 2024-06-12 10:29:55
    /// </summary>
    public class EquSparepartRecordEntity : BaseEntity
    {
        /// <summary>
        /// 备件id equ_sparepart的id
        /// </summary>
        public long SparepartId { get; set; }

       /// <summary>
        /// 备件类型Id;同一个备件只允许分配至一个备件类型中
        /// </summary>
        public long? SparePartTypeId { get; set; }

       /// <summary>
        /// 物料ID
        /// </summary>
        public long ProcMaterialId { get; set; }

       /// <summary>
        /// 备件/工装
        /// </summary>
        public int Type { get; set; }

       /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

       /// <summary>
        /// 是否关键备件
        /// </summary>
        public YesOrNoEnum IsKey { get; set; }

       /// <summary>
        /// 是否标准件; 1、是 2.否
        /// </summary>
        public YesOrNoEnum? IsStandard { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 图纸编号
        /// </summary>
        public string BluePrintNo { get; set; }

       /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }

       /// <summary>
        /// 管理方式
        /// </summary>
        public int? ManagementMode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 供应商ID;wh_supplier表的Id
        /// </summary>
        public long? SupplierId { get; set; }

       /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacturer { get; set; }

       /// <summary>
        /// 图纸编号
        /// </summary>
        public string DrawCode { get; set; }

       /// <summary>
        /// 规格型号
        /// </summary>
        public string Specifications { get; set; }

       /// <summary>
        /// 存放位置;自动带出
        /// </summary>
        public string Position { get; set; }

       /// <summary>
        /// 是否关键设备;0、否 1、是
        /// </summary>
        public bool? IsCritical { get; set; }

       /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

       /// <summary>
        /// 操作类型 1、注册 2、入库 3、出库4、绑定5、解绑
        /// </summary>
        public string OperationType { get; set; }

       /// <summary>
        /// 操作数量
        /// </summary>
        public decimal? OperationQty { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long? EquipmentId { get; set; }

       
    }
}
