using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码表，数据实体对象   
    /// manu_sfc
    /// @author wangkeming
    /// @date 2023-03-29 05:40:43
    /// </summary>
    public class ManuSfcEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量  
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        ///报废数量
        /// </summary>
        public decimal? ScrapQty { get; set; }

        /// <summary>
        /// 状态: 1：排队中 2：活动中 3：完成-在制 4：完成 5：锁定 6：报废 7：删除
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>   
        /// 条码类型 1、生产条码 2、非生产条码
        /// </summary>
        public SfcTypeEnum Type { get; set; } = SfcTypeEnum.Produce;

        /// <summary>
        /// 是否使用
        /// </summary>
        public YesOrNoEnum IsUsed { get; set; } = YesOrNoEnum.No;

        /// <summary>
        /// 报废表Id
        /// </summary>
        public long? SfcScrapId { get; set; }

        /// <summary>
        /// 备份字段  用户状态回撤
        /// </summary>
        public SfcStatusEnum StatusBack { get; set; }
    }
}
