﻿using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    public class UpdateIsScrapCommand
    {
        /// <summary>
        /// 产品条码列表
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum IsScrap { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}