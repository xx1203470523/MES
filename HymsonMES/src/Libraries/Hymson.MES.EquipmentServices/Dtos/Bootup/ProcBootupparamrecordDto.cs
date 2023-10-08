/*
 *creator: Karl
 *
 *describe: 开机参数采集表    Dto | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 开机参数采集表Dto
    /// </summary>
    public record ProcBootupparamrecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 配方Id
        /// </summary>
        public string RecipeId { get; set; }

       /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 上限值
        /// </summary>
        public string ParamUpper { get; set; }

       /// <summary>
        /// 下限值
        /// </summary>
        public string ParamLower { get; set; }

       /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 开机参数采集表新增Dto
    /// </summary>
    public record ProcBootupparamrecordCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 配方Id
        /// </summary>
        public string RecipeId { get; set; }

       /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 上限值
        /// </summary>
        public string ParamUpper { get; set; }

       /// <summary>
        /// 下限值
        /// </summary>
        public string ParamLower { get; set; }

       /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 开机参数采集表更新Dto
    /// </summary>
    public record ProcBootupparamrecordModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 配方Id
        /// </summary>
        public string RecipeId { get; set; }

       /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

       /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }

       /// <summary>
        /// 上限值
        /// </summary>
        public string ParamUpper { get; set; }

       /// <summary>
        /// 下限值
        /// </summary>
        public string ParamLower { get; set; }

       /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 开机参数采集表分页Dto
    /// </summary>
    public class ProcBootupparamrecordPagedQueryDto : PagerInfo
    {
    }
}
