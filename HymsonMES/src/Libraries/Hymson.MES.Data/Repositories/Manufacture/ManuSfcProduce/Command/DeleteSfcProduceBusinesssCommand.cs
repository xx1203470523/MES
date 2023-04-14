using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    /// <summary>
    /// 删除在制品业务实体类
    /// </summary>
    public class DeleteSfcProduceBusinesssCommand
    {
        /// <summary>
        /// 删除条码id
        /// </summary>
        public IEnumerable<long> SfcInfoIds { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }
    }
}
