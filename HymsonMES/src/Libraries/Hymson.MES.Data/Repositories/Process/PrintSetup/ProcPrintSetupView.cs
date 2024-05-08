using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;


namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcPrintSetupView : ProcPrintSetupEntity
    {
        /// <summary>
        /// type
        /// 空值 : false  
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料ID
        /// 空值 : false  
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料ID
        /// 空值 : false  
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 物料ID
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 物料ID
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 关联的物料列表
        /// </summary>
        public List<ProcMaterialEntity> procMaterialEntity { get; set; }

        /// <summary>
        /// 关联的资源列表
        /// </summary>
        public List<ProcResourceEntity> procResourceEntity { get; set; }
    }
}
