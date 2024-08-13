using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.XnebulaWMS
{
    /// <summary>
    /// MES接口的传参
    /// </summary>
    public class StockMesNIODto
    {
        /// <summary>
        ///  物料编码
        /// </summary>
        public string MaterialCode { get; set; }
    }

    ///// <summary>
    ///// 接口的传参
    ///// </summary>
    //public class StockMesNIODto
    //{
    //    /// <summary>
    //    ///  物料编码
    //    /// </summary>
    //    public IEnumerable<string> MaterialCodes { get; set; }

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    /// <param name="materialCodes"></param>
    //    public StockMesNIODto()
    //    {
    //        MaterialCodes = new List<string>();
    //    }
    //}

    /// <summary>
    /// M接口的传参
    /// </summary>
    public class StockMesDataDto
    {
        /// <summary>
        ///  开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///  结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
