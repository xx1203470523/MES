using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Common
{
    /// <summary>
    /// 状态变更
    /// </summary>
    public record ChangeStatusDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 需要变更为的状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

    }
}
