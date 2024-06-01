using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 工作中心表Dto
    /// </summary>
    public record InteWorkCenterDto : BaseEntityDto
    {
        /// <summary>
        /// 工作中心编码 
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 工作中心名称 
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 工作中心类型(车间/产线,WorkCenterTypeEnum) 
        /// </summary>
        public WorkCenterTypeEnum WorkCenterType { get; set; }

        /// <summary>
        /// 工作中心描述 
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 上级工作中心(车间) 
        /// </summary>
        public string? WorkShopCode { get; set; }

    }
}
