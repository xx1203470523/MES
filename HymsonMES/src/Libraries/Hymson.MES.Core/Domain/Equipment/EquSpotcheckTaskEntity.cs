using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（点检任务）   
    /// equ_spotcheck_task
    /// @author User
    /// @date 2024-05-14 09:00:47
    /// </summary>
    public class EquSpotcheckTaskEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
