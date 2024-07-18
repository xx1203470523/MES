using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests
{
    /// <summary>  
    /// 订单工作单详情实体类  
    /// </summary>  
    public class RotorWorkOrderRequest
    {
        /// <summary>  
        /// 订单号  
        /// </summary>  
        public string OrderNo { get; set; } = string.Empty;

        /// <summary>  
        /// 工单号  
        /// </summary>  
        public string WorkNo { get; set; } = string.Empty;

        /// <summary>  
        /// 物料编码  
        /// </summary>  
        public string ItemNo { get; set; } = string.Empty;

        /// <summary>  
        /// 生产数量  
        /// </summary>  
        //public int WorkNum { get; set; }

        /// <summary>  
        /// 工单总数量不能为空  
        /// </summary>  
        public int WorkTotal { get; set; }

        /// <summary>  
        /// 版本号  
        /// </summary>  
        public int VersionNo { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductTypeNO { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>  
        /// 产线UID  
        /// </summary>  
        //public string LineUID { get; set; } = string.Empty; // 假设这里可以容忍空字符串作为默认值  

        /// <summary>  
        /// 状态  
        /// 默认为'S'  
        /// </summary>  
        //public string WorkStatus { get; set; } = "S";

        /// <summary>  
        /// 是否完成投入  
        /// 默认为false（如果未特别说明为true）  
        /// </summary>  
        //public bool InputStatus { get; set; } = false;

        /// <summary>  
        /// 计划时间  
        /// </summary>  
        public string PlanTime { get; set; } = string.Empty; // 假设这里可以容忍空字符串作为默认值  

        ///// <summary>  
        ///// 开始时间  
        ///// </summary>  
        //public string StartTime { get; set; } = string.Empty;

        ///// <summary>  
        ///// 结束时间  
        ///// </summary>  
        //public string EndTime { get; set; } = string.Empty;

        ///// <summary>  
        ///// 备注  
        ///// </summary>  
        //public string Remark { get; set; } = string.Empty; // 假设这里可以容忍空字符串作为默认值  

        ///// <summary>  
        ///// 是否主工单  
        ///// 默认为false（因为0在C#中视为false）  
        ///// </summary>  
        //public bool IsHostWork { get; set; } = false;
    }

}
