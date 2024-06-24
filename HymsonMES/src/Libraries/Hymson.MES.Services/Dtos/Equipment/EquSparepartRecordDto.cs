/*
 *creator: Karl
 *
 *describe: 设备备件记录表    Dto | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-14 04:03:04
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表Dto 
    /// </summary>
    public record EquSparepartRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public bool Type { get; set; }

       /// <summary>
        /// 单位ID
        /// </summary>
        public long UnitId { get; set; }

       /// <summary>
        /// 是否关键备件
        /// </summary>
        public bool IsKey { get; set; }

       /// <summary>
        /// 是否标准件;0、否 1、是
        /// </summary>
        public bool? IsStandard { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public bool Status { get; set; }

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
        public bool? ManagementMode { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

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

       /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

       /// <summary>
        /// 工作中心编码
        /// </summary>
        public string WorkCenterCode { get; set; }

       /// <summary>
        /// 领用人
        /// </summary>
        public string Recipients { get; set; }

       
    }

    /// <summary>
    /// 备件记录
    /// </summary>
    public record EquSparepartRecordPagedViewDto : BaseEntityDto
    {
        /// <summary>
        /// 备件编号 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string SparePartTypeCode { get; set; }

        /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; }

        /// <summary>
        /// 操作数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string RecordWorkCenterCode { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 操作类型 1、注册 2、入库 3、出库4、绑定5、解绑
        /// </summary> 
        public EquOperationTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 领用人
        /// </summary>
        public string Recipients { get; set; } 

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string CreatedBy { get; set; }
    }

    /// <summary>
    /// 设备备件记录表分页Dto
    /// </summary>
    public class EquSparepartRecordPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 备件编号 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作类型 1、注册 2、入库 3、出库4、绑定5、解绑
        /// </summary> 
        public EquOperationTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime[] CreatedOn { get; set; }
    }
}
