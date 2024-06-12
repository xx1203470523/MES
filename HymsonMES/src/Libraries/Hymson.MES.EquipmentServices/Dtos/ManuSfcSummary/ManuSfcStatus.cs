using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos;

/// <summary>
/// 条码生产状态记录查询
/// </summary>
public class ManuSfcStatusQueryDto
{
    /// <summary>
    /// 查询条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 查询工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }
}

/// <summary>
/// 条码生产状态记录
/// </summary>
public class ManuSfcStatusOutputDto
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    public int? QualityStatus { get; set; }

    /// <summary>
    ///一次合格品（是，否）
    /// </summary>
    public int? FirstQualityStatus { get; set; }
}