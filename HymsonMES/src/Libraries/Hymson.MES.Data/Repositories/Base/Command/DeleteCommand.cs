﻿namespace Hymson.MES.Data.Repositories.Common.Command;

/// <summary>
/// 删除实体
/// @author wangkeming
/// @date 2023-02-11 04:45:25
/// </summary>
public class DeleteCommand
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 主键Ids
    /// </summary>
    public IEnumerable<long> Ids { get; set; }

    /// <summary>
    /// 操作人员
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime DeleteOn { get; set; }
}
