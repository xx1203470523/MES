/*
 *creator: Karl
 *
 *describe: 载具注册表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具注册表 分页参数
    /// </summary>
    public class InteVehiclePagedQuery : PagerInfo
    {
        public long SiteId { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public EnableEnum? Status { get; set; }

        /// <summary>
        /// 载具类型编码
        /// </summary>
        public string? VehicleTypeCode { get; set; }
    }
}
