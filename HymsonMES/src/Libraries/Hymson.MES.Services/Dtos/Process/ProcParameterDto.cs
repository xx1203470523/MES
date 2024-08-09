using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;
using MimeKit;

using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 标准参数表Dto
    /// </summary>
    public record ProcParameterDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public string ParameterUnit { get; set; } = "";

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否是CC项
        /// </summary>
        public TrueOrFalseEnum IsCc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否是Sc项
        /// </summary>
        public TrueOrFalseEnum IsSc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否是SPC项目
        /// </summary>
        public TrueOrFalseEnum IsSpc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 未维护
        /// </summary>
        public Category01Enum Category01 { get; set; } = Category01Enum.No;

        /// <summary>
        /// 是否推送
        /// </summary>
        public TrueOrFalseEnum IsPush { get; set; } = TrueOrFalseEnum.No;
    }

    /// <summary>
    /// 
    /// </summary>
    public record CustomProcParameterDto : ProcParameterDto
    {
        /// <summary>
        /// 类型   1 设备  2 产品  3 设备+产品  4 环境  等等
        /// </summary>
        public ParameterTypeEnum[] Type { get; set; }
    }


    /// <summary>
    /// 标准参数表新增Dto
    /// </summary>
    public record ProcParameterCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 参数代码
        /// </summary>
        public string ParameterCode { get; set; } = "";

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; } = "";

        /// <summary>
        /// 参数单位
        /// </summary>
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; } = "";

    }

    /// <summary>
    /// 标准参数表更新Dto
    /// </summary>
    public record ProcParameterModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 是否是CC项
        /// </summary>
        public TrueOrFalseEnum IsCc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否是Sc项
        /// </summary>
        public TrueOrFalseEnum IsSc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否是SPC项目
        /// </summary>
        public TrueOrFalseEnum IsSpc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 未维护
        /// </summary>
        public Category01Enum Category01 { get; set; } = Category01Enum.No;

        /// <summary>
        /// 是否推送
        /// </summary>
        public TrueOrFalseEnum IsPush { get; set; } = TrueOrFalseEnum.No;
    }

    /// <summary>
    /// 标准参数表分页Dto
    /// </summary>
    public class ProcParameterPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string? ParameterCode { get; set; } = "";

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string? ParameterName { get; set; } = "";

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum? DataType { get; set; }

    }

    /// <summary>
    /// 参数导入模板模型
    /// </summary>
    public record ProcParameterImportDto : BaseExcelDto
    {
        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "标准参数编码(必填)", Order = 1)]
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "标准参数名称(必填)", Order = 2)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        [EpplusTableColumn(Header = "参数单位", Order = 3)]
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        [EpplusTableColumn(Header = "数据类型(必填)", Order = 4)]
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 5)]
        public string? Remark { get; set; }

    }

    public class ParameterExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 参数导出
    /// </summary>
    public record ProcParameterExportDto : BaseExcelDto
    {
        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "标准参数编码(必填)", Order = 1)]
        public string ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        [EpplusTableColumn(Header = "标准参数名称(必填)", Order = 2)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        [EpplusTableColumn(Header = "参数单位(必填)", Order = 3)]
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        [EpplusTableColumn(Header = "数据类型(必填)", Order = 4)]
        public string DataType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [EpplusTableColumn(Header = "备注", Order = 5)]
        public string? Remark { get; set; }

    }
}
