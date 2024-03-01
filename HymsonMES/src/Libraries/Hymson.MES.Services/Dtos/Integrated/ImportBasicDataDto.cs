using Hymson.Infrastructure;
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
        public string UseStatus { get; set; }

        /// <summary>
        /// 使用状态(枚举，EquipmentTypeEnum根据枚举文本去判断取值)
        /// </summary>
        [EpplusTableColumn(Header = "设备类型", Order = 5)]
        public string? EquipmentType { get; set; }

        /// <summary>
        /// 根据输入的部门编码去匹配到部门Id
        /// </summary>
        [EpplusTableColumn(Header = "使用部门", Order = 6)]
        public string? UseDepartment { get; set; }

        /// <summary>
        /// 入厂日期
        /// </summary>
        [EpplusTableColumn(Header = "入厂日期", Order = 7)]
        public DateTime? EntryDate { get; set; }

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
}
