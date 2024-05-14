using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 点检任务新增/更新Dto
    /// </summary>
    public record EquSpotcheckTaskSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 点检任务Dto
    /// </summary>
    public record EquSpotcheckTaskDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 点检任务分页Dto
    /// </summary>
    public class EquSpotcheckTaskPagedQueryDto : PagerInfo { }

}
