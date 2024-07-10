using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Entity
{
    /// <summary>
    /// 工单产品关联
    /// </summary>
    public class WorkOrderListEntity
    {
        /// <summary>
        /// UID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        public int WorkNum { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string WorkStatus { get; set; }

        /// <summary>
        /// 是否是返修
        /// </summary>
        public bool ReworkStatus { get; set; }

        /// <summary>
        /// 产品结果状态
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建人编码
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 更新人编码
        /// </summary>
        public string UpdateID { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateName { get; set; }
    }
}
