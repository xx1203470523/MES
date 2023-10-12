﻿namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 模组CCS信息
    /// </summary>
    public class CirculationModuleCCSInfoDto
    {
        /// <summary>
        /// 模组型号
        /// </summary>
        public string? ModelCode { get; set; }

        /// <summary>
        /// 模组条码
        /// </summary>
        public string? SFC { get; set; }
        /// <summary>
        /// CCS位置/型号
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// 模组是否存在NG
        /// </summary>
        public bool IsNg { get; set; } = false;
    }

    /// <summary>
    /// 获取NG
    /// </summary>
    public class NgCirculationModuleCCSInfoDto: CirculationModuleCCSInfoDto
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId {  get; set; }
    }

    /// <summary>
    /// 补料确认输入
    /// </summary>
    public class ReplenishInputDto
    {
        /// <summary>
        /// 模组条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
