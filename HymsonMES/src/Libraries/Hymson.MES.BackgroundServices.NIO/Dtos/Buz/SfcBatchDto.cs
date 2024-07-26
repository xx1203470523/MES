using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>
    /// 条码批次信息
    /// </summary>
    public class SfcBatchDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 批次码
        /// </summary>
        public string Batch { get; set; }
    }
}
