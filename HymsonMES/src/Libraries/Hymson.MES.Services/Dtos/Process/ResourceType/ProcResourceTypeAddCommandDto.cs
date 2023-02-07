using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process.ResourceType
{
    public record ProcResourceTypeAddCommandDto : BaseEntityDto
    {
        /// <summary>
        /// 描述 :资源类型 
        /// 空值 : false  
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联的资源Id
        /// </summary>
        public IEnumerable<string> ResourceIds { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
