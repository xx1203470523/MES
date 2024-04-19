using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;
using System.Security.Cryptography.X509Certificates;

namespace Hymson.MES.Services.Dtos.Integrated
{
    public record class ImportBasicDataDto
    {

    }

    public record class ImportEquipmentDto : BaseExcelDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码(必填)", Order = 1)]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称(必填)", Order = 2)]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        [EpplusTableColumn(Header = "存放位置(必填)", Order = 3)]
        public string Location { get; set; }

        /// <summary>
        /// 使用状态(枚举，EquipmentUseStatusEnum根据枚举文本去判断取值)
        /// </summary>
        [EpplusTableColumn(Header = "使用状态(必填)", Order = 4)]
        public EquipmentUseStatusEnum UseStatus { get; set; }

        /// <summary>
        /// 使用状态(枚举，EquipmentTypeEnum根据枚举文本去判断取值)
        /// </summary>
        [EpplusTableColumn(Header = "设备类型", Order = 5)]
        public EquipmentTypeEnum? EquipmentType { get; set; }

        /// <summary>
        /// 根据输入的部门编码去匹配到部门Id
        /// </summary>
        [EpplusTableColumn(Header = "使用部门", Order = 6)]
        public string? UseDepartment { get; set; }

        /// <summary>
        /// 入厂日期
        /// </summary>
        [EpplusTableColumn(Header = "入厂日期", Order = 7)]
        public string? EntryDate { get; set; }

        /// <summary>
        /// 质保期限(月)
        /// </summary>
        [EpplusTableColumn(Header = "质保期限(月)", Order = 8)]
        public int? QualTime { get; set; }

        /// <summary>
        /// 描述 :厂商 
        /// </summary>
        [EpplusTableColumn(Header = "厂商", Order = 9)]
        public string? Manufacturer { get; set; } = "";

        /// <summary>
        /// 描述 :供应商 
        /// </summary>
        [EpplusTableColumn(Header = "供应商", Order = 10)]
        public string? Supplier { get; set; } = "";

        /// <summary>
        /// 根据输入的设备组编码去匹配到设备组Id
        /// </summary>
        [EpplusTableColumn(Header = "设备组编码", Order = 11)]
        public string? EquipmentGroup{ get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        [EpplusTableColumn(Header = "功率", Order = 12)]
        public string Power { get; set; } = "";

        /// <summary>
        /// 能耗等级 
        /// </summary>
        [EpplusTableColumn(Header = "能耗等级", Order = 13)]
        public string EnergyLevel { get; set; } = "";

        /// <summary>
        /// IP
        /// </summary>
        [EpplusTableColumn(Header = "IP", Order = 14)]
        public string? Ip { get; set; } = "";

        /// <summary>
        /// 描述 :备注 
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 15)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 描述 :设备描述  
        /// </summary>
        [EpplusTableColumn(Header = "设备描述", Order = 16)]
        public string EquipmentDesc { get; set; } = "";
    }

    public record class ImportResourceDto : BaseExcelDto
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码(必填)", Order = 1)]
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [EpplusTableColumn(Header = "资源名称(必填)", Order = 2)]
        public string ResName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 3)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 资源类型(去资源类型表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "资源类型", Order = 4)]
        public string? ResType { get; set; }

        /// <summary>
        /// 设备编码(去设备表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 5)]
        public string? EquipmentCode { get; set; }

        /// <summary>
        /// 是否主设备
        /// </summary>
        [EpplusTableColumn(Header = "是否主设备", Order = 6)]
        public TrueOrFalseEnum? IsMain { get; set; }
    }

    public record class ImportResourceTypeDto : BaseExcelDto
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        [EpplusTableColumn(Header = "资源类型(必填)", Order = 1)]
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型名称
        /// </summary>
        [EpplusTableColumn(Header = "资源类型名称(必填)", Order = 2)]
        public string ResTypeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 3)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 资源编码(去资源表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 4)]
        public string? ResCode { get; set; }
    }

    public record class ImportProcedureDto : BaseExcelDto
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [EpplusTableColumn(Header = "类型", Order = 3)]
        public ProcedureTypeEnum Type { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 4)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 资源类型(去资源类型表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "资源类型", Order = 5)]
        public string? ResType { get; set; }

        /// <summary>
        /// 是否维修返回
        /// </summary>
        [EpplusTableColumn(Header = "是否维修返回", Order = 6)]
        public TrueOrFalseEnum? IsRepairReturn { get; set; }
    }

    public record class ImportWorkLineDto : BaseExcelDto
    {
        /// <summary>
        /// 线体编码
        /// </summary>
        [EpplusTableColumn(Header = "线体编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 线体名称
        /// </summary>
        [EpplusTableColumn(Header = "线体名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 是否混线
        /// </summary>
        [EpplusTableColumn(Header = "是否混线(必填)", Order = 3)]
        public TrueOrFalseEnum? IsMixLine { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 4)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 资源编码(去资源表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 5)]
        public string? ResCode { get; set; }
    }

    public record class ImportWorkShopDto : BaseExcelDto
    {
        /// <summary>
        /// 车间编码
        /// </summary>
        [EpplusTableColumn(Header = "车间编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        [EpplusTableColumn(Header = "车间名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 3)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 线体编码(去工作重心表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "线体编码", Order = 5)]
        public string? LineCode { get; set; }
    }

    public record class ImportMaterialDto : BaseExcelDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        [EpplusTableColumn(Header = "物料编码(必填)", Order = 1)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [EpplusTableColumn(Header = "物料名称(必填)", Order = 2)]
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        [EpplusTableColumn(Header = "物料版本(必填),默认1.0", Order = 3)]
        public string Version { get; set; } = "1.0";

        /// <summary>
        /// 批次大小(去工作重心表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "批次大小(必填)", Order = 4)]
        public decimal Batch { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        [EpplusTableColumn(Header = "采购类型(必填)", Order = 5)]
        public MaterialBuyTypeEnum BuyType { get; set; }

        /// <summary>
        /// 数据收集方式
        /// </summary>
        [EpplusTableColumn(Header = "数据收集方式(必填)", Order = 6)]
        public MaterialSerialNumberEnum SerialNumber { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        [EpplusTableColumn(Header = "工艺路线编码", Order = 7)]
        public string? ProcessRouteCode { get; set; }

        /// <summary>
        /// Bom编码
        /// </summary>
        [EpplusTableColumn(Header = "Bom编码", Order = 8)]
        public string? BomCode { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        [EpplusTableColumn(Header = "标包数量", Order = 9)]
        public int? PackageNum { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        [EpplusTableColumn(Header = "物料描述", Order = 10)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 计量单位
        /// </summary>
        [EpplusTableColumn(Header = "计量单位", Order = 11)]
        public string? Unit { get; set; }

        /// <summary>
        /// 基于时间
        /// </summary>
        [EpplusTableColumn(Header = "基于时间", Order = 12)]
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 消耗公差
        /// </summary>
        [EpplusTableColumn(Header = "消耗公差", Order = 13)]
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 是否默认版本 
        /// </summary>
        [EpplusTableColumn(Header = "是否默认版本", Order = 14)]
        public TrueOrFalseEnum? IsDefaultVersion { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        [EpplusTableColumn(Header = "消耗系数", Order = 15)]
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 验证掩码组
        /// </summary>
        [EpplusTableColumn(Header = "验证掩码组", Order = 16)]
        public string? ValidationMaskGroup { get; set; }
    }

    public record class ImportMaterialGroupDto : BaseExcelDto
    {
        /// <summary>
        /// 物料组编码
        /// </summary>
        [EpplusTableColumn(Header = "物料组编码(必填)", Order = 1)]
        public string GroupCode { get; set; }

        /// <summary>
        /// 物料组名称
        /// </summary>
        [EpplusTableColumn(Header = "物料组名称(必填)", Order = 2)]
        public string GroupName { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        [EpplusTableColumn(Header = "物料组描述", Order = 3)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 关联物料(去物料表中匹配)
        /// </summary>
        [EpplusTableColumn(Header = "关联物料", Order = 5)]
        public string? MaterialCode { get; set; }
    }

    public record class ImportParameterDto : BaseExcelDto
    {
        /// <summary>
        /// 描述 :参数代码 
        /// </summary>
        [EpplusTableColumn(Header = "标准参数编码(必填)", Order = 1)]
        public string ParameterCode { get; set; }

        /// <summary>
        /// 描述 :标准参数名称 
        /// </summary>
        [EpplusTableColumn(Header = "标准参数名称(必填)", Order = 2)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        [EpplusTableColumn(Header = "数据类型(必填)", Order = 3)]
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        [EpplusTableColumn(Header = "参数单位", Order = 4)]
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 5)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 是否环境参数
        /// </summary>
        [EpplusTableColumn(Header = "是否环境参数", Order = 6)]
        public TrueOrFalseEnum? IsEnvironment { get; set; }

        /// <summary>
        /// 是否设备参数
        /// </summary>
        [EpplusTableColumn(Header = "是否设备参数", Order = 7)]
        public TrueOrFalseEnum? IsEquipment { get; set; }

        /// <summary>
        /// 是否品质参数
        /// </summary>
        [EpplusTableColumn(Header = "是否品质参数", Order = 8)]
        public TrueOrFalseEnum? IsIQC { get; set; }

        /// <summary>
        /// 是否产品参数
        /// </summary>
        [EpplusTableColumn(Header = "是否产品参数", Order = 9)]
        public TrueOrFalseEnum? IsProduct { get; set; }
    }
}
