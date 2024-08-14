using Hymson.MES.HttpClients.Requests.ERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Responses.NioErp
{
    /// <summary>
    /// ERP中关于NIO的返回
    /// </summary>
    public class NioErpResponse : BaseERPResponse
    {
        /// <summary>
        /// 数据详情
        /// </summary>
        public List<NioErpDetail> Data { get; set; } = new List<NioErpDetail>();
    }

    public class NioErpDetail
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupperialCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupperialName { get; set; }

        /// <summary>
        /// 请购单号
        /// </summary>
        public string BuyReqCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        //数量
        public decimal Num { get; set; }

        //需要到货日期
        public string ReceiveData { get; set; }
    }
}
