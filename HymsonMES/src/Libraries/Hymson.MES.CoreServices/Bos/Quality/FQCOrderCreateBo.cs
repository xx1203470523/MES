﻿using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Events.Quality;

namespace Hymson.MES.CoreServices.Bos.Quality
{
    public class FQCOrderManualCreateBo : CoreBaseBo
    {
        /// <summary>
        /// 成品产出记录Id列表
        /// </summary>
        public IEnumerable<long> RecordIds { get; set; }
    }

    public class FQCOrderAutoCreateAutoBo : CoreBaseBo
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 条码类型(1-托盘 2-栈板 3-SFC 4-箱)
        /// </summary>
        public FQCLotUnitEnum CodeType { get; set; }

        public IEnumerable<RecordDetailInfo>? RecordDetails { get; set; }
    }
    public class FQCOrderAutoCreateAutoResponse
    {
        public List<QualFinallyOutputRecordEntity> QualFinallyOutputRecords { get; set; }
        public List<QualFinallyOutputRecordDetailEntity>  QualFinallyOutputRecordDetailEntities { get; set; }
        public FQCOrderAutoCreateIntegrationEvent FQCOrderAutoCreateIntegrationEvent { get; set; }
        public List<FQCOrderAutoCreateIntegrationEvent> FQCOrderAutoCreateIntegrationEvents { get; set; }
    }

    public class RecordDetailInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public long WorkCenterId { get; set; }
    }
}
