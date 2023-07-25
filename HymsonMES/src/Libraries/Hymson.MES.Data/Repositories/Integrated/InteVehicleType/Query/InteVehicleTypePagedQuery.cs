/*
 *creator: Karl
 *
 *describe: 载具类型维护 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具类型维护 分页参数
    /// </summary>
    public class InteVehicleTypePagedQuery : PagerInfo
    {
        public long SiteId { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public DisableOrEnableEnum? Status { get; set; }
    }
}
