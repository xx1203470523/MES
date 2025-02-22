﻿namespace Hymson.MES.EquipmentServices.Dtos
{
    /// <summary>
    /// 出站（单条码）
    /// </summary>
    public record OutBoundDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 是否合格
        /// 0不合格,1合格
        /// </summary>
        public int? IsQualified { get; set; }

        /*
        /// <summary>
        /// 出站参数
        /// </summary>
        public OutBoundParam[]? ParamList { get; set; }
        */

        /// <summary>
        /// 绑定的物料批次条码列表（消耗条码）
        /// </summary>
        public string[]? ConsumeCodes { get; set; }

        /// <summary>
        /// Ng代码
        /// </summary>
        public NGParameterBo[]? FailInfo { get; set; }

        /// <summary>
        /// 是否过站
        /// </summary>
        public bool IsPassingStation { get; set; } = false;

    }

    /// <summary>
    /// 出站（单条码）
    /// </summary>
    public record OutBoundItemDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 是否合格
        /// 0不合格,1合格
        /// </summary>
        public int? IsQualified { get; set; }

        /*
        /// <summary>
        /// 出站参数
        /// </summary>
        public OutBoundParam[]? ParamList { get; set; }
        */

        /// <summary>
        /// 绑定的物料批次条码列表（消耗条码）
        /// </summary>
        public string[]? ConsumeCodes { get; set; }

        /// <summary>
        /// Ng代码
        /// </summary>
        public NGParameterBo[]? FailInfo { get; set; }

        /// <summary>
        /// 是否过站
        /// </summary>
        public bool IsPassingStation { get; set; } = false;

    }

    /// <summary>
    /// 出站（多条码）
    /// </summary>
    public record OutBoundMoreDto : BaseDto
    {
        /// <summary>
        /// 产品条码集合
        /// </summary>
        public OutBoundItemDto[] SFCs { get; set; }
    }

    /// <summary>
    /// 出站（单载具）
    /// </summary>
    public record OutBoundCarrierDto : BaseDto
    {
        /// <summary>
        /// 载具编码
        /// </summary>
        public string CarrierNo { get; set; } = string.Empty;

        /// <summary>
        /// 是否合格
        /// 0不合格,1合格
        /// </summary>
        public int? IsQualified { get; set; }

        /*
        /// <summary>
        /// 出站参数
        /// </summary>
        public OutBoundParam[]? ParamList { get; set; }
        */

        /// <summary>
        /// 绑定的物料批次条码列表（消耗条码）
        /// </summary>
        public string[]? ConsumeCodes { get; set; }

        /// <summary>
        /// Ng代码
        /// </summary>
        public NGParameterBo[]? FailInfo { get; set; }

        /// <summary>
        /// 是否过站
        /// </summary>
        public bool IsPassingStation { get; set; } = false;

    }



    /// <summary>
    /// 
    /// </summary>
    public record NGParameterBo
    {
        /// <summary>
        /// NG代码
        /// </summary>
        public string NCCode { get; set; } = string.Empty;
    }

}
