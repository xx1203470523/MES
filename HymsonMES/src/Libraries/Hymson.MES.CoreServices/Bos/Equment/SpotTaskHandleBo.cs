using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Equment
{
    public class SpotTaskHandleBo
    {
        /// <summary>
        /// 处理方式
        /// </summary>
        public EquSpotcheckTaskProcessedEnum HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

}
