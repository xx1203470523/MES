/*
 *creator: Karl
 *
 *describe: 二维载具条码明细    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wxk
 *build datetime: 2023-07-19 08:14:38
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 二维载具条码明细，数据实体对象   
    /// inte_vehice_freight_stack
    /// @author wxk
    /// @date 2023-07-19 08:14:38
    /// </summary>
    public class InteVehiceFreightStackEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 载具位置id
        /// </summary>
        public long LocationId { get; set; }

       /// <summary>
        /// 装载条码
        /// </summary>
        public string BarCode { get; set; }

       
    }
}
