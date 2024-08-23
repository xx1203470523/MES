/*
 *creator: Karl
 *
 *describe: 操作面板按钮作业    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 03:34:48
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 操作面板按钮作业Dto
    /// </summary>
    public record ManuFacePlateButtonJobRelationDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 按钮Id
        /// </summary>
        public long FacePlateButtonId { get; set; }

        /// <summary>
        /// 作业Id
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 作业编码
        /// </summary>
        public string JobCode { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 是否清除条码
        /// </summary>
        public YesOrNoEnum IsClear { get; set; }

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
    }


    /// <summary>
    /// 操作面板按钮作业新增Dto
    /// </summary>
    public record ManuFacePlateButtonJobRelationCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 按钮Id
        /// </summary>
        public long FacePlateButtonId { get; set; }

        /// <summary>
        /// 作业Id
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否清除条码
        /// </summary>
        public YesOrNoEnum IsClear { get; set; }
    }

    /// <summary>
    /// 操作面板按钮作业更新Dto
    /// </summary>
    public record ManuFacePlateButtonJobRelationModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 按钮Id
        /// </summary>
        public long FacePlateButtonId { get; set; }

        /// <summary>
        /// 作业Id
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否清除条码
        /// </summary>
        public YesOrNoEnum IsClear { get; set; }
    }

    /// <summary>
    /// 操作面板按钮作业分页Dto
    /// </summary>
    public class ManuFacePlateButtonJobRelationPagedQueryDto : PagerInfo
    {
    }
}
