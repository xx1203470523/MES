using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query
{
    public class SfcProduceBusinessQuery
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

        /// <summary>
        /// 业务类型;1、返修业务  2、锁业务
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }
    }
}
