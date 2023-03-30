/*
 *creator: Karl
 *
 *describe: 条码接收    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 条码接收，数据实体对象   
    /// manu_sfc_info
    /// @author pengxin
    /// @date 2023-03-21 04:33:58
    /// </summary>
    public class PlanSfcPrintView : BaseEntity
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }


        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; }


        /// <summary>
        /// 使用状态
        /// </summary>
        public PlanSFCUsedStatusEnum IsUsed { get; set; }


        /// <summary>
        /// 打印状态
        /// </summary>
        public PlanSFCPrintStatusEnum PrintStatus { get; set; }

    }
}
