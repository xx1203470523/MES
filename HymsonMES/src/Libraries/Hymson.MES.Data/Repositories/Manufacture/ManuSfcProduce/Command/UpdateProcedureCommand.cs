﻿using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    /// <summary>
    ///更新工序
    /// </summary>
    public class UpdateProcedureCommand
    {
        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcProduceStatusEnum Status { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
    /// <summary>
    /// 根据SFC批量更新工序与状态 (参数)
    /// </summary>
    public class UpdateProcedureAndStatusCommand
    {
        public long SiteId { get; set; }

        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///SFC
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcProduceStatusEnum? Status { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }


    /// <summary>
    /// 根据SFC批量更新工序与状态 (参数)
    /// </summary>
    public class UpdateProcedureAndResourceCommand
    {
        public long SiteId { get; set; }

        /// <summary>
        ///SFC
        /// </summary>
        public string[] Sfcs { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; } 
        
        /// <summary>
        /// 资源
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        public static implicit operator List<object>(UpdateProcedureAndResourceCommand v)
        {
            throw new NotImplementedException();
        }
    }
}
