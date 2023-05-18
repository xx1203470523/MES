/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:41:16
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码步骤ng信息记录表，数据实体对象   
    /// manu_sfc_step_ng
    /// @author zhaoqing
    /// @date 2023-03-18 05:41:16
    /// </summary>
    public class ManuSfcStepNgEntity : BaseEntity
    {
        /// <summary>
        /// 步骤表id
        /// </summary>
        public long BarCodeStepId { get; set; }

       /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
