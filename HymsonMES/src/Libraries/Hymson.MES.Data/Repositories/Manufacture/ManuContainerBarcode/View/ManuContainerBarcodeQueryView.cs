using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuContainerBarcodeQueryView : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 包装等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 最大包装数
        /// </summary>
        public decimal Maximum { get; set; }

        /// <summary>
        /// 最小包装数
        /// </summary>
        public decimal Minimum { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 容器规格id
        /// </summary>
        public long ContainerId { get; set; }

        /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public int Status { get; set; }
    }
}
