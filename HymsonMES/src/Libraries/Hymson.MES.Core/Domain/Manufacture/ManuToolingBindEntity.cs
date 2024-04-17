using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Common;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（工装条码绑定）   
    /// manu_tooling_bind
    /// @author xiaofei
    /// @date 2024-04-11 07:15:53
    /// </summary>
    public class ManuToolingBindEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工装编码
        /// </summary>
        public string ToolingCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 0-解绑 1-绑定
        /// </summary>
        public BindStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
