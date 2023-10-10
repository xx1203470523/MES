/*
 *creator: Karl
 *
 *describe: 开机参数表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 开机参数表，数据实体对象   
    /// proc_bootupparam
    /// @author wxk
    /// @date 2023-07-05 04:22:20
    /// </summary>
    public class ProcBootupparamEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 开机配方Id
        /// </summary>
        public long RecipeId { get; set; }

       /// <summary>
        /// 标准参数Id
        /// </summary>
        public long ParamId { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       
    }
}
