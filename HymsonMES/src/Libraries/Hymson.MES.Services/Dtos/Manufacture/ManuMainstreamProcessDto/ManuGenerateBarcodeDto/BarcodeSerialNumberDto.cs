﻿using Hymson.MES.Core.Enums.Integrated;
using Hymson.Sequences.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto
{
    /// <summary>
    /// 流水号生成实体
    /// </summary>
    public class BarcodeSerialNumberDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string CodeRuleKey { get; set; }

        /// <summary>
        /// 起始
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 基数;10 16 32
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// 忽略字符
        /// </summary>
        public string IgnoreChar { get; set; }

        /// <summary>
        /// 增量
        /// </summary>
        public int Increment { get; set; }

        /// <summary>
        /// 序列长度;0:表示无限长度
        /// </summary>
        public int OrderLength { get; set; }
     
        /// <summary>
        /// 重置序号;1：从不；2：每天；3：每周；4：每月；5：每年；
        /// </summary>
        public SerialNumberTypeEnum ResetType { get; set; }
    }
}
