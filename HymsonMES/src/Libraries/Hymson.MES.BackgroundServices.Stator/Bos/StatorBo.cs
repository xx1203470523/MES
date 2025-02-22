﻿using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;

namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 基类（响应）
    /// </summary>
    public class StatorSummaryBo
    {
        /// <summary>
        /// 集合（条码）
        /// </summary>
        public BaseStatorBo StatorBo { get; set; } = new();

        /// <summary>
        /// 集合（定子条码）
        /// </summary>
        public List<StatorBarCodeEntity> AddStatorBarCodeEntities { get; set; } = new();

        /// <summary>
        /// 集合（定子条码）
        /// </summary>
        public List<StatorBarCodeEntity> UpdateStatorBarCodeEntities { get; set; } = new();

        /// <summary>
        /// 集合（铜线条码）
        /// </summary>
        public List<WireBarCodeEntity> AddWireBarCodeEntities { get; set; } = new();

        /// <summary>
        /// 集合（铜线条码）
        /// </summary>
        public List<WireBarCodeEntity> UpdateWireBarCodeEntities { get; set; } = new();

        /// <summary>
        /// 集合（定子铜线关系）
        /// </summary>
        public List<StatorWireRelationEntity> AddStatorWireRelationEntities { get; set; } = new();


        /// <summary>
        /// 集合（条码）
        /// </summary>
        public List<ManuSfcEntity> ManuSFCEntities { get; set; } = new();

        /// <summary>
        /// 集合（条码信息）
        /// </summary>
        public List<ManuSfcInfoEntity> ManuSFCInfoEntities { get; set; } = new();

        /// <summary>
        /// 集合（步骤信息）
        /// </summary>
        public List<ManuSfcStepEntity> ManuSfcStepEntities { get; set; } = new();

        /// <summary>
        /// 集合（物料库存）
        /// </summary>
        public List<WhMaterialInventoryEntity> UpdateWhMaterialInventoryEntities { get; set; } = new();

        /// <summary>
        /// 集合（物料台账）
        /// </summary>
        public List<WhMaterialStandingbookEntity> WhMaterialStandingbookEntities { get; set; } = new();

        /// <summary>
        /// 集合（流转信息）
        /// </summary>
        public List<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new();

        /// <summary>
        /// 集合（产品不良记录）
        /// </summary>
        public List<ManuProductBadRecordEntity> ManuProductBadRecordEntities { get; set; } = new();

        /// <summary>
        /// 集合（产品NG记录）
        /// </summary>
        public List<ManuProductNgRecordEntity> ManuProductNgRecordEntities { get; set; } = new();


        /// <summary>
        /// 集合（标准参数）
        /// </summary>
        public List<ProcParameterEntity> ProcParameterEntities { get; set; } = new();

        /// <summary>
        /// 集合（产品参数）
        /// </summary>
        public List<Core.Domain.Parameter.ManuProductParameterEntity> ManuProductParameterEntities { get; set; } = new();

    }

}
