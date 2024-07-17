using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Entity
{
    /// <summary>
    /// 上料实体
    /// </summary>
    [Table("Work_ItemInfo")]
    public class WorkItemInfoEntity
    {
        /// <summary>
        /// UID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 过程UID
        /// </summary>
        public string ProcessUID { get; set; }

        /// <summary>
        /// 产品编号唯一
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public string WorkPosNo { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string MatName { get; set; }

        /// <summary>
        /// 物料扫码结果
        /// </summary>
        public string MatValue { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MatCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string MatBatchCode { get; set; }

        /// <summary>
        /// 批次号对应物料编码
        /// </summary>
        public string LotCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? MatNum { get; set; }

        /// <summary>
        /// 物料状态，默认值为1
        /// </summary>
        public int MatStatus { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 物料序列号
        /// </summary>
        public int? MatSerialID { get; set; }
    }
}
