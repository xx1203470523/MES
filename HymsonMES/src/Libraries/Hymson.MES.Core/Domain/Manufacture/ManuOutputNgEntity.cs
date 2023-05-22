/*
 *creator: Karl
 *
 *describe: 产出上报NG    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-05-19 10:47:15
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 产出上报NG，数据实体对象   
    /// manu_output_ng
    /// @author pengxin
    /// @date 2023-05-19 10:47:15
    /// </summary>
    public class ManuOutputNgEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// NGId
        /// </summary>
        public long NGId { get; set; }

       /// <summary>
        /// NG代码
        /// </summary>
        public string NGCode { get; set; }

       /// <summary>
        /// NG数量
        /// </summary>
        public decimal NGQty { get; set; }

       
    }
}
