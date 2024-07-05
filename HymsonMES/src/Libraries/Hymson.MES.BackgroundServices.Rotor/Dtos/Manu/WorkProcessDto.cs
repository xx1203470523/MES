using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Dtos.Manu
{
    /// <summary>
    /// 过站表
    /// </summary>
    public class WorkProcessDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 描述 : 产品编号唯一
        /// 对应MES里面的条码
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 描述 : 工位编码
        /// 需要将设备的工位编码转为工序并进行处理
        /// </summary>
        public string WorkPosNo { get; set; }

        /// <summary>
        /// 描述 : 结果
        /// OK 或者 NG
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 描述 : 产品状态
        /// Z- 已完成（下工站排队） R- 未开始（异常情况下，从头开工）S- 进行中（进站）
        /// </summary>
        public string ProductStatus { get; set; }

        /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否复投
        /// </summary>
        public bool IsRepeat { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 描述 : 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 描述 : 创建人工号
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 描述 : 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 描述 : 修改人工号
        /// </summary>
        public string UpdateID { get; set; }

        /// <summary>
        /// 描述 : 托盘号
        /// 目前用不到
        /// </summary>
        public string TrayID { get; set; }

        /// <summary>
        /// 描述 : 工艺时间
        /// 目前给的数据有问题，这个字段不能用
        /// </summary>
        public float? CycleTime { get; set; }
    }
}
