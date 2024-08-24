using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Mavel.Stator
{
    /// <summary>
    /// 数据实体（转子条码记录）   
    /// manu_stator_barcode
    /// @author Yxx
    /// @date 2024-08-24 02:36:31
    /// </summary>
    public class ManuStatorBarcodeEntity : BaseEntity
    {
        /// <summary>
        /// 内定子ID
        /// </summary>
        public long InnerId { get; set; }

        /// <summary>
        /// 内定子编码
        /// </summary>
        public string InnerBarCode { get; set; }

        /// <summary>
        /// 外定子编码
        /// </summary>
        public string OuterBarCode { get; set; }

        /// <summary>
        /// BusBar编码
        /// </summary>
        public string BusBarCode { get; set; }

        /// <summary>
        /// 槽底纸编码
        /// </summary>
        public string PaperBottomLotBarcode { get; set; }

        /// <summary>
        /// 槽盖纸编码
        /// </summary>
        public string PaperTopLotBarcode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string ProductionCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
