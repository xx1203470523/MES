using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// IPQC检验项目参数新增/更新Dto
    /// </summary>
    public record QualIpqcInspectionParameterSaveDto : BaseEntityDto
    {
       /// <summary>
        /// 全检参数明细Id
        /// </summary>
        public long InspectionParameterGroupDetailId { get; set; }

       /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 录入次数
        /// </summary>
        public int? EnterNumber { get; set; }

       /// <summary>
        /// 是否设备采集;1、是 2、否
        /// </summary>
        public YesOrNoEnum? IsDeviceCollect { get; set; }

       /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence { get; set; }

       
    }

    /// <summary>
    /// IPQC检验项目参数Dto
    /// </summary>
    public record QualIpqcInspectionParameterDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// IPQC检验项目Id qual_ipqc_inspection 的id
        /// </summary>
        public long IpqcInspectionId { get; set; }

       /// <summary>
        /// 全检参数明细Id
        /// </summary>
        public long InspectionParameterGroupDetailId { get; set; }

       /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public DataTypeEnum DataType { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 录入次数
        /// </summary>
        public int? EnterNumber { get; set; }

        /// <summary>
        /// 是否设备采集;1、是 2、否
        /// </summary>
        public YesOrNoEnum? IsDeviceCollect { get; set; } = YesOrNoEnum.No;

       /// <summary>
       /// 顺序
       /// </summary>
        public int Sequence { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// IPQC检验项目参数分页Dto
    /// </summary>
    public class QualIpqcInspectionParameterPagedQueryDto : PagerInfo { }

}
