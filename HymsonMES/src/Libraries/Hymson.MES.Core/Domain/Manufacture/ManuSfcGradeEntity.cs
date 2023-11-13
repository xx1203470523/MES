/*
 *creator: Karl
 *
 *describe: 条码档位表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:16
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码档位表，数据实体对象   
    /// manu_sfc_grade
    /// @author zhaoqing
    /// @date 2023-07-27 01:54:16
    /// </summary>
    public class ManuSfcGradeEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 最终挡位值
        /// </summary>
        public string Grade { get; set; }

       
    }
}
