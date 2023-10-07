using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数表 分页参数
    /// </summary>
    public class ProcParameterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string? ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string? ParameterName { get; set; }

        /// <summary>
        /// 参数单位（字典定义）
        /// </summary>
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum? DataType { get; set; }

        /// <summary>
        /// 描述（标准参数）
        /// </summary>
        public string? Remark { get; set; }
    }
}
