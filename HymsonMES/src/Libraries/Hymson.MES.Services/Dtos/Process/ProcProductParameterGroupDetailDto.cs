using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 产品检验参数项目表新增/更新Dto
    /// </summary>
    public record ProcProductParameterGroupDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 全检检验参数id
        /// </summary>
        public long ParameterGroupId { get; set; }

        /// <summary>
        /// 参数id（产品参数）
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; } = DataTypeEnum.Text;

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public TrueOrFalseEnum IsRequired { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 顺序（正整数，最大值10000）
        /// </summary>
        public int Sort { get; set; }
    }

    /// <summary>
    /// 产品检验参数项目表Dto
    /// </summary>
    public record ProcProductParameterGroupDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 产品检验参数id
        /// </summary>
        public long ParameterGroupId { get; set; }

        /// <summary>
        /// 参数id（产品参数）
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// 空值 : false  
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; } = DataTypeEnum.Text;

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public TrueOrFalseEnum IsRequired { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 顺序（正整数，最大值10000）
        /// </summary>
        public int Sort { get; set; }

    }

}
