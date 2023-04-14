/*
 *creator: Karl
 *
 *describe: 条码步骤业务表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wangkeming
 *build datetime: 2023-04-06 08:31:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码步骤业务表，数据实体对象   
    /// maun_sfc_step_business
    /// @author wangkeming
    /// @date 2023-04-06 08:31:40
    /// </summary>
    public class MaunSfcStepBusinessEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public long SfcStepId { get; set; }

       /// <summary>
        /// 1、锁业务
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }

       /// <summary>
        /// 业务内容
        /// </summary>
        public string BusinessContent { get; set; }
    }
}
