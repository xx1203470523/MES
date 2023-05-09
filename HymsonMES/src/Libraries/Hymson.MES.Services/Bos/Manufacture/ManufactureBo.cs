﻿using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManufactureBo
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /*
        /// <summary>
        /// 额外参数
        /// </summary>
        public string Extra { get; set; }
        */
    }

    /// <summary>
    /// 在制维修出站
    /// </summary>
    public class ManufactureRepairBo
    {
        /// <summary>
        /// 当前工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 返回工序ID
        /// </summary>
        public long ReturnProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }
    }

    /// <summary>
    /// 扣料
    /// </summary>
    public class MaterialDeductBo : MaterialDeductItemBo
    {
        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 替代料集合
        /// </summary>
        public IEnumerable<MaterialDeductItemBo> ReplaceMaterials { get; set; }

    }

    /// <summary>
    /// 扣料项
    /// </summary>
    public class MaterialDeductItemBo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal? Loss { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        public decimal ConsumeRatio { get; set; } = 1;
    }


    /// <summary>
    /// 容器包装
    /// </summary>
    public class ManufactureContainerBo
    {
        /// <summary>
        /// 容器ID
        /// </summary>
        public long ContainerId { get; set; }
    }
}
