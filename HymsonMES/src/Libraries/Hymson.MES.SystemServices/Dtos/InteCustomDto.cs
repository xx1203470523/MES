/*
 *creator: Karl
 *
 *describe: 客户维护    Dto | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */

using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 客户维护Dto
    /// </summary>
    public record InteCustomDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 客户编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

       /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

       /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

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
    }


    /// <summary>
    /// 客户维护新增Dto
    /// </summary>
    public record InteCustomCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 客户编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Describe { get; set; }

       /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

       /// <summary>
        /// 电话
        /// </summary>
        public string? Telephone { get; set; }

       
    }

    /// <summary>
    /// 客户维护更新Dto
    /// </summary>
    public record InteCustomModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Describe { get; set; }

       /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

       /// <summary>
        /// 电话
        /// </summary>
        public string? Telephone { get; set; }

       
    }

    /// <summary>
    /// 客户维护分页Dto
    /// </summary>
    public class InteCustomPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 客户维护导入模板模型
    /// </summary>
    public record InteCustomImportDto : BaseExcelDto
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        [EpplusTableColumn(Header = "客户编码(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [EpplusTableColumn(Header = "客户名称(必填)", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 3)]
        public string? Describe { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [EpplusTableColumn(Header = "地址", Order = 4)]
        public string? Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [EpplusTableColumn(Header = "电话", Order = 5)]
        public string? Telephone { get; set; }
    }

    public record InteCustomExportDto : BaseExcelDto
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        [EpplusTableColumn(Header = "客户编码", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [EpplusTableColumn(Header = "客户名称", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EpplusTableColumn(Header = "描述", Order = 3)]
        public string? Describe { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [EpplusTableColumn(Header = "地址", Order = 4)]
        public string? Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [EpplusTableColumn(Header = "电话", Order = 5)]
        public string? Telephone { get; set; }
    }

    public class InteCustomExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }
}
