/*
 *creator: Karl
 *
 *describe: 时间通配（转换） 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-10-13 06:33:21
 */

using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 时间通配（转换） 查询参数
    /// </summary>
    public class InteTimeWildcardQuery
    {
    }

    /// <summary>
    /// 根据编码与类型查询
    /// </summary>
    public class InteTimeWildcardCodeAndTypeQuery 
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 类型： 年  月 日
        /// </summary>
        public TimeWildcardTypeEnum Type { get; set; }
    }
}
