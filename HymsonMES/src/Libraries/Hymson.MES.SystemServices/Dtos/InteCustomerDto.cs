using Hymson.Infrastructure;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 客户维护Dto
    /// </summary>
    public record InteCustomerDto : BaseEntityDto
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; } = "";

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = "";

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; } = "";

    }

}
