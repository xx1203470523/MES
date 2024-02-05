﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.OutBound
{
    /// <summary>
    /// 出站参数
    /// </summary>
    public class OutBoundParam
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; } = string.Empty;

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = string.Empty;

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 标准上限
        /// </summary>
        public string? StandardUpperLimit { get; set; }

        /// <summary>
        /// 标准下限
        /// </summary>
        public string? StandardLowerLimit { get; set; }

        /// <summary>
        /// 判定结果
        /// </summary>
        public string? JudgmentResult { get; set; }

        /// <summary>
        /// 测试时长
        /// </summary>
        public string? TestDuration { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public string? TestTime { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public string? TestResult { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string? CreatedBy { get; set; }
    }

    /// <summary>
    /// NG代码
    /// </summary>
    public class OutBoundNG
    {
        /// <summary>
        /// NG代码
        /// </summary>
        public string NGCode { get; set; } = string.Empty;
    }
}
