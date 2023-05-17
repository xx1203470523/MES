/*
 *creator: Karl
 *
 *describe: 条码绑定关系表    Dto | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:11
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 条码绑定关系表Dto
    /// </summary>
    public record ManuSfcBindDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 绑定条码
        /// </summary>
        public string BindSFC { get; set; }

       /// <summary>
        /// 条码绑定类型;1：模组绑定电芯 2：绑定模组
        /// </summary>
        public int? Type { get; set; }

       /// <summary>
        /// 绑定状态;0-解绑;1-绑定
        /// </summary>
        public long Status { get; set; }

       /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindingTime { get; set; }

       /// <summary>
        /// 解绑时间
        /// </summary>
        public DateTime? UnbindingTime { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public int? Location { get; set; }

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
    /// 条码绑定关系表新增Dto
    /// </summary>
    public record ManuSfcBindCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 绑定条码
        /// </summary>
        public string BindSFC { get; set; }

       /// <summary>
        /// 条码绑定类型;1：模组绑定电芯 2：绑定模组
        /// </summary>
        public int? Type { get; set; }

       /// <summary>
        /// 绑定状态;0-解绑;1-绑定
        /// </summary>
        public long Status { get; set; }

       /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindingTime { get; set; }

       /// <summary>
        /// 解绑时间
        /// </summary>
        public DateTime? UnbindingTime { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public int? Location { get; set; }

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
    /// 条码绑定关系表更新Dto
    /// </summary>
    public record ManuSfcBindModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 绑定条码
        /// </summary>
        public string BindSFC { get; set; }

       /// <summary>
        /// 条码绑定类型;1：模组绑定电芯 2：绑定模组
        /// </summary>
        public int? Type { get; set; }

       /// <summary>
        /// 绑定状态;0-解绑;1-绑定
        /// </summary>
        public long Status { get; set; }

       /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime BindingTime { get; set; }

       /// <summary>
        /// 解绑时间
        /// </summary>
        public DateTime? UnbindingTime { get; set; }

       /// <summary>
        /// 位置号
        /// </summary>
        public int? Location { get; set; }

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
    /// 条码绑定关系表分页Dto
    /// </summary>
    public class ManuSfcBindPagedQueryDto : PagerInfo
    {
    }
}
