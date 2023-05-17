/*
 *creator: Karl
 *
 *describe: 操作面板按钮 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:58:19
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板按钮 分页参数
    /// </summary>
    public class ManuFacePlateButtonPagedQuery : PagerInfo
    {
        public long SiteId { get; set; }

        public long? FacePlateId { get; set; }
    }
}
