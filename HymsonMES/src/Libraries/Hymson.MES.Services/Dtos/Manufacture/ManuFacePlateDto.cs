/*
 *creator: Karl
 *
 *describe: 操作面板    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 操作面板Dto
    /// </summary>
    public record ManuFacePlateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 类型;1、生产过站；2、在制品维修
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

       /// <summary>
        /// 状态;1、新建、2、启用、3、保留、4、废除；
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }


    /// <summary>
    /// 操作面板新增Dto
    /// </summary>
    public record ManuFacePlateCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 类型;1、生产过站；2、在制品维修
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

       /// <summary>
        /// 状态;1、新建、2、启用、3、保留、4、废除；
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 操作面板更新Dto
    /// </summary>
    public record ManuFacePlateModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 类型;1、生产过站；2、在制品维修
        /// </summary>
        public ManuFacePlateTypeEnum Type { get; set; }

       /// <summary>
        /// 状态;1、新建、2、启用、3、保留、4、废除；
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 会话时间（分钟）
        /// </summary>
        public int ConversationTime { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       

    }

    /// <summary>
    /// 操作面板分页Dto
    /// </summary>
    public class ManuFacePlatePagedQueryDto : PagerInfo
    {
    }
}
