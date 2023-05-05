using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.View
{
    public class ManuSfcProduceBusinessView : ManuSfcProduceBusinessEntity
    {
        /// <summary>
        /// SFC
        /// </summary>
        public string Sfc { get; set; }
    }
}
