using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

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
        public ParameterUnitEnum ParameterUnit { get; set; }

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

    }

    /// <summary>
    /// 
    /// </summary>
    public record CustomProcParameterDto : ProcParameterDto
    {
        /// <summary>
        /// 类型   1 设备  2 产品  3 设备+产品  4 环境  等等
        /// </summary>
        public ParameterTypeShowEnum Type { get; set; }
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
        /// 参数单位（字典定义）
        /// </summary>
        public ParameterUnitEnum ParameterUnit { get; set; }

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

        /*
        /// <summary>
        /// 参数代码（这里传值不会修改，仅仅是校验用）
        /// </summary>
        public string ParameterCode { get; set; } = "";
        */

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; } = "";

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public ParameterUnitEnum ParameterUnit { get; set; }

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
        public ParameterUnitEnum? ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum? DataType { get; set; }

        /// <summary>
        /// 描述（标准参数）
        /// </summary>
        public string? Remark { get; set; } = "";

    }
}
