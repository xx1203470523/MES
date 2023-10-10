﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport;

/// <summary>
/// 流转表追溯信息
/// </summary>
public record ManuSfcCirculationViewDto : BaseEntityDto
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// 当前工序id
    /// </summary>
    public long ProcedureId { get; set; }
    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcedureCode { get; set; }
    /// <summary>
    /// 工序名称
    /// </summary>
    public string ProcedureName { get; set; }

    /// <summary>
    /// 资源id
    /// </summary>
    public long? ResourceId { get; set; }

    /// <summary>
    /// 资源编码
    /// </summary>
    public string ResourceCode { get; set; }
    /// <summary>
    /// 资源名称
    /// </summary>
    public string ResourceName { get; set; }

    /// <summary>
    /// 设备id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public string EquipentCode { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    public string EquipentName { get; set; }

    /// <summary>
    /// 扣料上料点id
    /// </summary>
    public long? FeedingPointId { get; set; }

    /// <summary>
    /// 流转前条码
    /// </summary>
    public string SFC { get; set; }

    /// <summary>
    /// 流转前工单id
    /// </summary>
    public long WorkOrderId { get; set; }
    /// <summary>
    /// 工单编码
    /// </summary>
    public string WorkOrderCode { get; set; }

    /// <summary>
    /// 流转前产品id
    /// </summary>
    public long ProductId { get; set; }
    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 流转后条码信息
    /// </summary>
    public string CirculationBarCode { get; set; }

    /// <summary>
    /// 流转条码数量
    /// </summary>
    public decimal? CirculationQty { get; set; }

    /// <summary>
    /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
    /// </summary>
    public SfcCirculationTypeEnum CirculationType { get; set; }

    /// <summary>
    /// 是否拆解
    /// </summary>
    public TrueOrFalseEnum IsDisassemble { get; set; } = TrueOrFalseEnum.No;

    /// <summary>
    /// 合并时绑定位置
    /// </summary>
    public string? Location { get; set; }
    /// <summary>
    /// 创建人
    /// </summary>
    public string CreatedBy { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }
}
