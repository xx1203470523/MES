/*
 *creator: Karl
 *
 *describe: 条码绑定解绑记录表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码绑定解绑记录表，数据实体对象   
    /// manu_sfc_bind_record
    /// @author chenjianxiong
    /// @date 2023-05-17 10:09:25
    /// </summary>
    public class ManuSfcBindRecordEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 绑定条码
        /// </summary>
        public string BindSFC { get; set; }

       /// <summary>
        /// 条码绑定类型;1：模组绑定电芯 2：绑定模组
        /// </summary>
        public int? Type { get; set; }

       /// <summary>
        /// 绑定状态;0-解绑 1-绑定
        /// </summary>
        public ManuSfcBindStatusEnum OperationType { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public int? Location { get; set; }

       
    }
}
