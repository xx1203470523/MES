/*
 *creator: Karl
 *
 *describe: 操作面板 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:50:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板 分页参数
    /// </summary>
    public class ManuFacePlatePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 类型
        /// </summary>
        public ManuFacePlateTypeEnum? Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
