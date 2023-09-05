using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备故障原因表新增Dto
    /// </summary>
    public record EquFaultReasonSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; }

        ///// <summary>
        ///// 故障原因状态（字典定义）
        ///// </summary>
        //public SysDataStatusEnum? UseStatus { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

    }

    /// <summary>
    /// 设备故障原因表Dto
    /// </summary>
    public record EquFaultReasonDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障原因代码
        /// </summary>
        public string FaultReasonCode { get; set; }

        /// <summary>
        /// 故障原因名称
        /// </summary>
        public string FaultReasonName { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? UseStatus { get; set; }

        /// <summary>
        /// 说明
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
        public bool? IsDeleted { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public record CustomEquFaultReasonDto : EquFaultReasonDto
    {

    }

    /// <summary>
    /// 设备故障原因表分页Dto
    /// </summary>
    public class EquFaultReasonPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码（设备故障原因）
        /// </summary>
        public string? FaultReasonCode { get; set; }

        /// <summary>
        /// 名称（设备故障原因）
        /// </summary>
        public string? FaultReasonName { get; set; }

        /// <summary>
        /// 故障原因状态（字典定义）
        /// </summary>
        public SysDataStatusEnum? UseStatus { get; set; }

        /// <summary>
        /// 描述（设备故障原因）
        /// </summary>
        public string? Remark { get; set; }
    }
}
