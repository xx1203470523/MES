/*
 *creator: Karl
 *
 *describe: 设备点检模板 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板 查询参数
    /// </summary>
    public class EquSpotcheckTemplateQuery
    {
        /// <summary>
        /// CODE
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }
    }
}
