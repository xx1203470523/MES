/*
 *creator: Karl
 *
 *describe: 设备参数组    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 设备参数组Dto
    /// </summary>
    public record ProcEquipmentGroupParamDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 配编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 参数组类型;1、开机参数 2、设备过程参数
        /// </summary>
        public EquipmentGroupParamTypeEnum Type { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 是否使用0 未使用，1使用
        /// </summary>
        public bool? IsUsed { get; set; }

       
    }

    public record ProcEquipmentGroupParamViewDto: ProcEquipmentGroupParamDto 
    {
        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string MaterialNameVersion { get; set; }

        public string ProcedureCode { get; set; }

        public string ProcedureName { get; set; }

        public string EquipmentGroupCode { get; set; }

        public string EquipmentGroupName { get; set; }
    }

    /// <summary>
    /// 设备参数组新增Dto
    /// </summary>
    public record ProcEquipmentGroupParamCreateDto : BaseEntityDto
    {
       /// <summary>
        /// 配编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 参数组类型;1、开机参数 2、设备过程参数
        /// </summary>
        public EquipmentGroupParamTypeEnum Type { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        public List<ProcEquipmentGroupParamDetailCreateDto> ParamList { get; set; }
    }

    /// <summary>
    /// 设备参数组更新Dto
    /// </summary>
    public record ProcEquipmentGroupParamModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 参数组类型;1、开机参数 2、设备过程参数
        /// </summary>
        public EquipmentGroupParamTypeEnum Type { get; set; }

       /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }


        public List<ProcEquipmentGroupParamDetailCreateDto> ParamList { get; set; }
    }

    /// <summary>
    /// 设备参数组分页Dto
    /// </summary>
    public class ProcEquipmentGroupParamPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }
    }

    /// <summary>
    /// 设备参数组分页Dto
    /// </summary>
    public class ProcEquipmentGroupParamDetailParamPagedQueryDto : PagerInfo
    {
        public long? ProductId { get; set; }

        public long? ProcedureId { get; set; }


    }

    /// <summary>
    /// 设备参数导入参数
    /// </summary>
    public record ProcEquipmentGroupParamDetailParamImportDto : BaseExcelDto
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        [EpplusTableColumn(Header = "参数集编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        [EpplusTableColumn(Header = "参数集名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [EpplusTableColumn(Header = "版本", Order = 3)]
        public string Version { get; set; }

        /// <summary>
        /// 功能类型;开机参数/设备过程参数
        /// </summary>
        [EpplusTableColumn(Header = "功能类型(必填,开机参数/设备过程参数)", Order = 4)]
        public EquipmentGroupParamTypeEnum Type { get; set; }
       
        /// <summary>
        /// 产品
        /// </summary>
        [EpplusTableColumn(Header = "产品编码(必填)", Order = 5)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        [EpplusTableColumn(Header = "工序编码(必填)", Order = 6)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工艺设备组编码
        /// </summary>
        [EpplusTableColumn(Header = "工艺设备组编码(必填)", Order = 7)]
        public string EquipmentGroupCode { get; set; }

        /// <summary>
        /// 标准参数编码
        /// </summary>
        [EpplusTableColumn(Header = "参数编码", Order = 8)]
        public string? ParamCode { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        [EpplusTableColumn(Header = "小数位数", Order = 9)]
        public int? DecimalPlaces { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        [EpplusTableColumn(Header = "规格上限", Order = 10)]
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        [EpplusTableColumn(Header = "规格下限", Order = 11)]
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 规格中心
        /// </summary>
        [EpplusTableColumn(Header = "规格中心", Order = 12)]
        public decimal? CenterValue { get; set; }
    }
}
