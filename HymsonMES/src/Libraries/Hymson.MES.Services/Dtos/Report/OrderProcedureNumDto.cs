using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 工单工序数量参数
    /// </summary>
    public class OrderProcedureNumDto
    {
        /// <summary>
        /// ERP订单
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// MES工单
        /// </summary>
        public string ?OrderCode { get; set; }

        /// <summary>
        /// 计划开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[] CreatedOn { get; set; }
    }

    /// <summary>
    /// 工单统计数量
    /// </summary>
    public class OrderProcedureNumAllResultDto
    {
        /// <summary>
        /// 详情
        /// </summary>
        public List<OrderProcedureNumResultDto> DetailList { get; set; } = new List<OrderProcedureNumResultDto>();

        /// <summary>
        /// 汇总
        /// </summary>
        //public List<OrderProcedureSumResultDto> SumList { get; set; } = new List<OrderProcedureSumResultDto>();
    }

    /// <summary>
    /// 工单工序详细数据
    /// </summary>
    public class OrderProcedureNumResultDto
    {
        /// <summary>
        /// ERP订单
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// MES工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        public decimal WorkHour { get; set; } = 8m;

        /// <summary>
        /// 平均节拍
        /// </summary>
        public decimal Beat
        {
            get
            {
                int sumSecond = (int)(WorkHour * 60 * 60);
                return sumSecond / Num;
            }
        }
    }

    /// <summary>
    /// 工单工序汇总数据
    /// </summary>
    public class OrderProcedureSumResultDto
    {
        /// <summary>
        /// ERP订单
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// MES工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        public decimal WorkHour { get; set; }

        /// <summary>
        /// 平均节拍
        /// </summary>
        public decimal Beat
        {
            get
            {
                int sumSecond = (int)(WorkHour * 60 * 60);
                return sumSecond / Num;
            }
        }
    }
}
