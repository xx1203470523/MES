/*
 *creator: Karl
 *
 *describe: 维修详情    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-04-13 09:51:31
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 维修详情，数据实体对象   
    /// manu_sfc_repair_detail
    /// @author pengxin
    /// @date 2023-04-13 09:51:31
    /// </summary>
    public class ManuSfcRepairDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 维修记录id
        /// </summary>
        public long SfcRepairId { get; set; }

        /// <summary>
        /// 产品不良录入id
        /// </summary>
        public long ProductBadId { get; set; }

        /// <summary>
        /// 维修方法
        /// </summary>
        public string RepairMethod { get; set; }

        /// <summary>
        /// 原因分析
        /// </summary>
        public string CauseAnalyse { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum IsClose { get; set; }

    }
}
