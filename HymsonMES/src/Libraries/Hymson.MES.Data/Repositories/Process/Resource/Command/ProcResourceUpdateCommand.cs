using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcResourceUpdateCommand : BaseEntity
    {
        /// <summary>
        /// 描述 :所属资源类型ID 
        /// 空值 : false  
        /// </summary>
        public long ResTypeId { get; set; }

        /// <summary>
        /// id列表
        /// </summary>
        public long[] IdsArr { get; set; }
    }
}
