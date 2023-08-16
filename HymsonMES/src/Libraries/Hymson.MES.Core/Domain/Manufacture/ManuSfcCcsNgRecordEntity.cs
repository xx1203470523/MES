/*
 *creator: Karl
 *
 *describe: CCS盖板NG记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-08-15 05:15:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// CCS盖板NG记录，数据实体对象   
    /// manu_sfc_ccsNg_record
    /// @author chenjianxiong
    /// @date 2023-08-15 05:15:40
    /// </summary>
    public class ManuSfcCcsNgRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 模组条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// NG位置;A,B,C,D等
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// NG状态;0 NG，1 已确认修复或重新生产
        /// </summary>
        public ManuSfcCcsNgRecordStatusEnum Status { get; set; }

       /// <summary>
        /// Ng代码
        /// </summary>
        public string NgCode { get; set; }

       /// <summary>
        /// Ng名称
        /// </summary>
        public string NgName { get; set; }

       
    }
}
