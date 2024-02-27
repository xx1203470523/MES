/*
 *creator: Karl
 *
 *describe: 条码档位明细表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:27
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码档位明细表，数据实体对象   
    /// manu_sfc_grade_detail
    /// @author zhaoqing
    /// @date 2023-07-27 01:54:27
    /// </summary>
    public class ManuSfcGradeDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码档位Id
        /// </summary>
        public long GadeId { get; set; }

       /// <summary>
        /// 待选工序Id
        /// </summary>
        public long ProduceId { get; set; }

       /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 档次
        /// </summary>
        public string Grade { get; set; }

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
        /// 包含最小值类型;1< 2.≤
        /// </summary>
        public ContainingTypeEnum? MinContainingType { get; set; }

        /// <summary>
        /// 包含最大值类型;1< 2.≥
        /// </summary>
        public ContainingTypeEnum? MaxContainingType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
