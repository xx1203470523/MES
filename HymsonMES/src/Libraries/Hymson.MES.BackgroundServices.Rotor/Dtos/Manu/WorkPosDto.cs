using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Dtos.Manu
{
    /// <summary>
    /// 工位信息
    /// </summary>
    public class WorkPosDto
    {
        /// <summary>
        /// 工位编码
        /// </summary>
        public string WorkPosNo { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string WorkPosName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set; }
    }
}
