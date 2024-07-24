using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 蔚来
    /// </summary>
    public record NIOAddDto : BaseEntityDto
    {
        /// <summary>
        /// 业务场景;这里不允许0的数据存在；
        /// </summary>
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<object> Data { get; set; } = new();

    }

}
