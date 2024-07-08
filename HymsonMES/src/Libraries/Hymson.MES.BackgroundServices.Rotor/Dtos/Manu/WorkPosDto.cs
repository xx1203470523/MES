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
        /// MES工序编码
        /// </summary>
        public string ProcedureCode
        {
            get
            {
                return WorkPosNo.Split('.')[0];
            }
        }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string WorkPosName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 工位类型
        /// 0-忽略 1- 进站 2-出站 4-进出站
        /// </summary>
        public int WorkPosType { get; set; }
    }
}
