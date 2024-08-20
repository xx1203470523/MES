/*
 *creator: Karl
 *
 *describe: job业务配置配置表    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 02:55:48
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// job业务配置配置表Dto
    /// </summary>
    public record InteJobBusinessRelationDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 1资源  2工序 3不合格代码
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 关联点
        /// </summary>
        public int LinkPoint { get; set; } = -1;

        /// <summary>
        /// 所属不合格代码ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }


    /// <summary>
    /// job业务配置配置表新增Dto
    /// </summary>
    public record InteJobBusinessRelationCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 关联点
        /// </summary>
        public ResourceJobLinkPointEnum LinkPoint { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; } = false;

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; } = "";
    }

    /// <summary>
    /// job业务配置配置表更新Dto
    /// </summary>
    public record InteJobBusinessRelationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 1资源  2工序 3不合格代码
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 所属不合格代码ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// job业务配置配置表分页Dto
    /// </summary>
    public class InteJobBusinessRelationPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 1资源  2工序 3不合格代码
        /// </summary>
        public string? BusinessType { get; set; }

        /// <summary>
        /// 关联的业务表的ID
        /// </summary>
        public long BusinessId { get; set; }
    }
}
