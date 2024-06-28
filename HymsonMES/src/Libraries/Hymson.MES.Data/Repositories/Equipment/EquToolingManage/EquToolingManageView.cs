using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Equipment
{
    public class EquToolingManageView : EquToolsEntity
    {

        /// <summary>
        /// 工具类型编码
        /// 空值 : false  
        /// </summary>
        public string ToolsTypeCode { get; set; }

        /// <summary>
        /// 工具类型名称
        /// 空值 : false  
        /// </summary>
        public string ToolsTypeName { get; set; }
    }
}
