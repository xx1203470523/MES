﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    public record ManuProductParameterReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        [EpplusTableColumn(Header = "条码", Order = 1)]
        public string SFC { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 6)]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称", Order = 7)]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 4)]
        public string ResCode { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        [EpplusTableColumn(Header = "资源名称", Order = 5)]
        public string ResName { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 2)]
        public string ProcedureCode { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 3)]
        public string ProcedureName { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        [EpplusTableColumn(Header = "参数编码", Order = 10)]
        public string ParameterCode { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        [EpplusTableColumn(Header = "参数名称", Order = 11)]
        public string ParameterName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [EpplusTableColumn(Header = "参数值", Order = 12)]
        public string Paramvalue { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        [EpplusTableColumn(Header = "上报时间", Order = 13)]
        public DateTime LocalTime { get; set; }


        #region 导出不需要这些字段 已注释（客户反馈导出信息不全，取消注释）

        /// <summary>
        /// 状态;1：触发 2、恢复
        /// </summary>
        [EpplusTableColumn(Header = "设备状态", Order = 14)]
        public EquipmentAlarmStatusEnum? Status { get; set; }

        ///// <summary>
        ///// 上限
        ///// </summary>
        [EpplusTableColumn(Header = "参数上限", Order = 15)]
        public string StandardUpperLimit { get; set; }

        ///// <summary>
        ///// 下限
        ///// </summary>
        [EpplusTableColumn(Header = "参数下限", Order = 16)]
        public string StandardLowerLimit { get; set; }

        /// <summary>
        /// 判定结果（LA梁工硬要添加只做展示）
        /// </summary>
        [EpplusTableColumn(Header = "判断结果", Order = 17)]
        public string JudgmentResult { get; set; }

        /// <summary>
        /// 测试持续时间（LA梁工硬要添加只做展示）
        /// </summary>
        [EpplusTableColumn(Header = "测试持续时间", Order = 18)]
        public string TestDuration { get; set; }

        /// <summary>
        /// 测试时间（LA梁工硬要添加只做展示）
        /// </summary>
        [EpplusTableColumn(Header = "测试时间", Order = 19)]
        public string TestTime { get; set; }

        /// <summary>
        /// 测试结果（LA梁工硬要添加只做展示）
        /// </summary>
        [EpplusTableColumn(Header = "测试结果", Order = 20)]
        public string TestResult { get; set; }

        #endregion
    }
}
