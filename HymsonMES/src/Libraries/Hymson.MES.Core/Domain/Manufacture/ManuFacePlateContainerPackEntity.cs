using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 包装面板实体
    /// </summary>
    public class ManuFacePlateContainerPackEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 面板Id
        /// </summary>
        public long FacePlateId { get; set; }
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 是否修改资源
        /// </summary>
        public bool IsResourceEdit { get; set; }
        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 是否修改工序
        /// </summary>
        public bool IsProcedureEdit { get; set; }
        /// <summary>
        /// JOBID
        /// </summary>
        public string ScanJobId { get; set; }
        /// <summary>
        /// 是否有成功提示音
        /// </summary>
        public bool? IsSuccessBeep { get; set; }
        /// <summary>
        /// 成功提示音地址
        /// </summary>
        public string SuccessBeepUrl { get; set; }
        /// <summary>
        /// 成功提示音时间（ms）
        /// </summary>
        public decimal? SuccessBeepTime { get; set; }
        /// <summary>
        /// 是否有错误提示音
        /// </summary>
        public bool? IsErrorBeep { get; set; }
        /// <summary>
        /// 错误提示音地址
        /// </summary>
        public string ErrorBeepUrl { get; set; }
        /// <summary>
        /// 错误提示音时间（ms）
        /// </summary>
        public decimal? ErrorBeepTime { get; set; }
        /// <summary>
        /// 是否允许不同版本物料
        /// </summary>
        public bool IsAllowDifferentMaterial { get; set; }
        /// <summary>
        /// 是否允许混工单
        /// </summary>
        public bool IsMixedWorkOrder { get; set; }
        /// <summary>
        /// 是否允许排队产品
        /// </summary>
        public bool IsAllowQueueProduct { get; set; }
        /// <summary>
        /// 是否允许完成产品
        /// </summary>
        public bool IsAllowCompleteProduct { get; set; }
        /// <summary>
        /// 是否允许活动产品
        /// </summary>
        public bool IsAllowActiveProduct { get; set; }
        /// <summary>
        /// 是否显示最小数量
        /// </summary>
        public bool IsShowMinQty { get; set; }
        /// <summary>
        /// 显示当前数量
        /// </summary>
        public bool IsShowCurrentQty { get; set; }
        /// <summary>
        /// 是否显示最大数量
        /// </summary>
        public bool IsShowMaxQty { get; set; }
        /// <summary>
        /// 合格颜色
        /// </summary>
        public string QualifiedColour { get; set; }
        /// <summary>
        /// 错误颜色
        /// </summary>
        public string ErrorsColour { get; set; }
        /// <summary>
        /// 是否显示日志
        /// </summary>
        public bool IsShowLog { get; set; }
    }
}
