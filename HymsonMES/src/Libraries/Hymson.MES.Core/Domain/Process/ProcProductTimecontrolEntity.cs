/*
 *creator: Karl
 *
 *describe: 产品工序时间    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:06
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 产品工序时间，数据实体对象   
    /// proc_product_timecontrol
    /// @author zhaoqing
    /// @date 2023-07-27 01:54:06
    /// </summary>
    public class ProcProductTimecontrolEntity : BaseEntity
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 缓存时间(分)
        /// </summary>
        public int CacheTime { get; set; }

       /// <summary>
        /// 预警时间(分)
        /// </summary>
        public int? WarningTime { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }   
    }
}
