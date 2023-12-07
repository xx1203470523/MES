using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query
{
    public   class ManuSfcStepStatisticQuery
    {
        /// <summary>
        /// 水位id
        /// </summary>
        public long StartWaterMarkId { set; get; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Rows { set; get; }
    }
}
