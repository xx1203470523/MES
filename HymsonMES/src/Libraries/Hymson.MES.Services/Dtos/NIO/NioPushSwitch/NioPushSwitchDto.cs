using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.NioPushSwitch
{
    /// <summary>
    /// 蔚来推送开关新增/更新Dto
    /// </summary>
    public record NioPushSwitchSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 场景代码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求路径（0:Get;1:Post;2:Put;3:Delete;）
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 业务场景;0：表示总开关；
        /// </summary>
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 是否启用推送;0：不推送；1：推送；
        /// </summary>
        public TrueOrFalseEnum IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 蔚来推送开关Dto
    /// </summary>
    public record NioPushSwitchDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 场景代码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求路径（0:Get;1:Post;2:Put;3:Delete;）
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 业务场景;0：表示总开关；
        /// </summary>
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 是否启用推送;0：不推送；1：推送；
        /// </summary>
        public TrueOrFalseEnum IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        public long IsDeleted { get; set; }
    }

    /// <summary>
    /// 蔚来推送开关分页Dto
    /// </summary>
    public class NioPushSwitchPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 业务场景;0：表示总开关；
        /// </summary>
        public BuzSceneEnum? BuzScene { get; set; }

        /// <summary>
        /// 是否启用推送;0：不推送；1：推送；
        /// </summary>
        public TrueOrFalseEnum? IsEnabled { get; set; }
    }

    /// <summary>
    /// 蔚来推送开关新增/更新Dto
    /// </summary>
    public record NioPushSwitchModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 场景代码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求路径（0:Get;1:Post;2:Put;3:Delete;）
        /// </summary>
        public int Method { get; set; }

        /// <summary>
        /// 业务场景;0：表示总开关；
        /// </summary>
        public BuzSceneEnum BuzScene { get; set; }

        /// <summary>
        /// 是否启用推送;0：不推送；1：推送；
        /// </summary>
        public TrueOrFalseEnum IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
