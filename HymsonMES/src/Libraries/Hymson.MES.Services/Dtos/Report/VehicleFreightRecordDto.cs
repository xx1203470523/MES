using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public class VehicleFreightRecordDto
    {
        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 托盘编码
        /// </summary>
        public string VehicleCode { get; set; }

        /// <summary>
        /// 绑定类型
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 解绑时间
        /// </summary>
        public string BindAndUnbindTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        public string Location { get; set; }
    }

    public class VehicleFreightRecordQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 托盘Id
        /// </summary>
        public long? VehicleId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 计划开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }

    public class VehicleFreightRecordExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    public record VehicleFreightRecordExportDto : BaseExcelDto
    {
        /// <summary>
        /// 工作中心
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 1)]
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 2)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 3)]
        public string ProcedureName { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 4)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 5)]
        public string ResourceCode { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 6)]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 托盘编码
        /// </summary>
        [EpplusTableColumn(Header = "载具编码", Order = 7)]
        public string VehicleCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [EpplusTableColumn(Header = "产品条码", Order = 8)]
        public string BarCode { get; set; }

        /// <summary>
        /// 位置号
        /// </summary>
        [EpplusTableColumn(Header = "托盘位置", Order = 9)]
        public string Location { get; set; }

        /// <summary>
        /// 绑定类型
        /// </summary>
        [EpplusTableColumn(Header = "类型", Order = 10)]
        public string OperateType { get; set; }

        /// <summary>
        /// 解绑时间
        /// </summary>
        [EpplusTableColumn(Header = "操作时间", Order = 11)]
        public string BindAndUnbindTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EpplusTableColumn(Header = "创建人", Order = 12)]
        public string CreateBy { get; set; }
    }
}
