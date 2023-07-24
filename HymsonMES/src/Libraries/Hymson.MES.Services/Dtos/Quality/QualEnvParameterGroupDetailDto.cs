using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 环境检验参数项目表新增/更新Dto
    /// </summary>
    public record QualEnvParameterGroupDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 环境检验参数id
        /// </summary>
        public long ParameterVerifyEnvId { get; set; }

        /// <summary>
        /// 参数id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal UpperLimit { get; set; }

        /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal LowerLimit { get; set; }

        /// <summary>
        /// 频率
        /// </summary>
        public bool Frequency { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int EntryCount { get; set; }

    }

    /// <summary>
    /// 环境检验参数项目表Dto
    /// </summary>
    public record QualEnvParameterGroupDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 环境检验参数id
        /// </summary>
        public long ParameterVerifyEnvId { get; set; }

       /// <summary>
        /// 参数id
        /// </summary>
        public long ParameterId { get; set; }

       /// <summary>
        /// 规格上限
        /// </summary>
        public decimal UpperLimit { get; set; }

       /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 规格下限
        /// </summary>
        public decimal LowerLimit { get; set; }

       /// <summary>
        /// 频率
        /// </summary>
        public bool Frequency { get; set; }

       /// <summary>
        /// 录入次数
        /// </summary>
        public int EntryCount { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

    }

    /// <summary>
    /// 环境检验参数项目表分页Dto
    /// </summary>
    public class QualEnvParameterGroupDetailPagedQueryDto : PagerInfo { }

}
