using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合格代码分页参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodePagedQuery : PagerInfo
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 描述 : 状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum? Degree { get; set; }

        /// <summary>
        /// 不合格代码组
        /// </summary>
        public IEnumerable<long> Ids {  get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string? OrUnqualifiedCode {  get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? OrUnqualifiedCodeName { get; set; }
    }
}
