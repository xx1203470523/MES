/*
 *creator: Karl
 *
 *describe: 产品不良录入 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */

using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 产品不良录入 查询参数
/// </summary>
public class ManuProductBadRecordQuery
{
    /// <summary>
    /// 不合格记录开关;1、开启  2、关闭
    /// </summary>
    public ProductBadRecordStatusEnum? Status { get; set; }

    /// <summary>
    /// 产品条码
    /// </summary>
    public string SFC { get; set; }
   
    /// <summary>
    /// 不合格代码类型
    /// </summary>
    public QualUnqualifiedCodeTypeEnum? Type { get; set; }

    /// <summary>
    /// 站点id
    /// </summary>
    public long? SiteId { get; set; }



    /// <summary>
    /// 
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }

    /// <summary>
    /// 所属站点代码组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 发现不良工序Id
    /// </summary>
    public long? FoundBadOperationId { get; set; }

    /// <summary>
    /// 发现不良工序Id组
    /// </summary>
    public IEnumerable<long>? FoundBadOperationIds { get; set; }


    /// <summary>
    /// 发现不良资源id
    /// </summary>
    public long? FoundBadResourceId { get; set; }

    /// <summary>
    /// 发现不良资源id组
    /// </summary>
    public IEnumerable<long>? FoundBadResourceIds { get; set; }


    /// <summary>
    /// 流出不良工序
    /// </summary>
    public long? OutflowOperationId { get; set; }

    /// <summary>
    /// 流出不良工序组
    /// </summary>
    public IEnumerable<long>? OutflowOperationIds { get; set; }


    /// <summary>
    /// 不合格代码Id
    /// </summary>
    public long? UnqualifiedId { get; set; }

    /// <summary>
    /// 不合格代码Id组
    /// </summary>
    public IEnumerable<long>? UnqualifiedIds { get; set; }

    /// <summary>
    /// 产品条码模糊条件
    /// </summary>
    public string? SFCLike { get; set; }


    /// <summary>
    /// 条码id
    /// </summary>
    public long? SfcInfoId { get; set; }

    /// <summary>
    /// 条码id组
    /// </summary>
    public IEnumerable<long>? SfcInfoIds { get; set; }


    /// <summary>
    /// 数量最小值
    /// </summary>
    public decimal? QtyMin { get; set; }

    /// <summary>
    /// 数量最大值
    /// </summary>
    public decimal? QtyMax { get; set; }

    /// <summary>
    /// 不合格记录开关;1、开启  2、关闭组
    /// </summary>
    public IEnumerable<long>? Statuss { get; set; }


    /// <summary>
    /// 不良来源;·1、设备复投不良  2、人工录入不良
    /// </summary>
    public long? Source { get; set; }

    /// <summary>
    /// 不良来源;·1、设备复投不良  2、人工录入不良组
    /// </summary>
    public IEnumerable<long>? Sources { get; set; }


    /// <summary>
    /// 处置结果;1、设备误判
    /// </summary>
    public long? DisposalResult { get; set; }

    /// <summary>
    /// 处置结果;1、设备误判组
    /// </summary>
    public IEnumerable<long>? DisposalResults { get; set; }


    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 备注模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 最后修改人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 最后修改人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 修改时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 修改时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }
}
