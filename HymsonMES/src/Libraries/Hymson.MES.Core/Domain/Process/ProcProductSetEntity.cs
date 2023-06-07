/*
 *creator: Karl
 *
 *describe: 工序和资源半成品产品设置表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-06-05 11:16:51
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工序和资源半成品产品设置表，数据实体对象   
    /// proc_product_set 
    /// @author zhaoqing
    /// @date 2023-06-05 11:16:51
    /// </summary>
    public class ProcProductSetEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 资源或者工序id
        /// </summary>
        public long SetPointId { get; set; }

       /// <summary>
        /// 半成品id
        /// </summary>
        public long SemiProductId { get; set; }
    }
}
