using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command
{
    /// <summary>
    /// 根据ID更新电芯码
    /// </summary>
    public class UpdateSfcByIdCommand : UpdateCommand
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 电芯码
        /// </summary>
        public string Sfc { get; set; }
    }
}
