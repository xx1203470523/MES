using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.NIO.NioPushCollection.View
{
    /// <summary>
    /// 推送参数+推送状态
    /// </summary>
    public class NioPushCollectionStatusView : NioPushCollectionEntity
    {
        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public TrueOrFalseEnum IsOk { get; set; } = TrueOrFalseEnum.Yes;

        /// <summary>  
        /// 上限值，可能来自t2表  
        /// </summary>  
        public decimal? UpperLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 中心值，可能来自t2表  
        /// </summary>  
        public decimal? CenterValue { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 下限值，可能来自t2表  
        /// </summary>  
        public decimal? LowerLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }
    }

    /// <summary>
    /// 重复数据
    /// </summary>
    public class NioPushCollectionRepeatView
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string VendorFieldCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 查询条码
    /// </summary>
    public class NioPushCollectionSfcView
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string VendorFieldCode { get; set; }
    }

}
