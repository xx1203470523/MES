using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.EquipmentServices.Dtos.ManuCommonDto
{
    /// <summary>
    /// 工艺路线工序 
    /// </summary>
    public class ProcessRouteProcedureDto
    {
        /// <summary>
        /// 描述 :所属工艺路线ID 
        /// 空值 : false  
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 描述 :序号( 程序生成) 
        /// 空值 : true  
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 描述 :抽检类型 
        /// 空值 : true  
        /// </summary>
        public ProcessRouteInspectTypeEnum? CheckType { get; set; }

        /// <summary>
        /// 描述 :抽检比例 
        /// 空值 : true  
        /// </summary>
        public int? CheckRate { get; set; }

        /// <summary>
        /// 描述 :是否报工 
        /// 空值 : true  
        /// </summary>
        public int IsWorkReport { get; set; }


        /// <summary>
        /// 描述 :工序BOM代码 
        /// 空值 : false  
        /// </summary>
        public string ProcedureCode { get; set; }= "";

        /// <summary>
        /// 描述 :工序BOM名称 
        /// 空值 : false  
        /// </summary>
        public string ProcedureName { get; set; } = "";

        /// <summary>
        /// 描述 :类型 
        /// 空值 : false  
        /// </summary>
        public ProcedureTypeEnum Type { get; set; }

        /// <summary>
        /// 描述 :包装等级（字典数据） 
        /// 空值 : true  
        /// </summary>
        public int? PackingLevel { get; set; }

        /// <summary>
        /// 描述 :所属资源类型ID 
        /// 空值 : true  
        /// </summary>
        public long? ResourceTypeId { get; set; }

        /// <summary>
        /// 描述 :循环次数 
        /// 空值 : true  
        /// </summary>
        public int? Cycle { get; set; }

        /// <summary>
        /// 描述 :是否维修返回 
        /// 空值 : true  
        /// </summary>
        public byte IsRepairReturn { get; set; }
    }
}
