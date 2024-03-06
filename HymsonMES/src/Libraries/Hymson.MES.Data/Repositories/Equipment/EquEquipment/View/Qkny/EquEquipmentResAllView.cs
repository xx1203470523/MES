using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.View
{
    /// <summary>  
    /// 根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息  
    /// </summary>  
    public class EquEquipmentResAllView
    {
        /// <summary>  
        /// 设备ID  
        /// </summary>  
        public long EquipmentId { get; set; }

        /// <summary>  
        /// 设备代码  
        /// </summary>  
        public string EquipmentCode { get; set; }

        /// <summary>  
        /// 设备名称  
        /// </summary>  
        public string EquipmentName { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>  
        /// 资源ID  
        /// </summary>  
        public long ResId { get; set; }

        /// <summary>  
        /// 资源代码  
        /// </summary>  
        public string ResCode { get; set; }

        /// <summary>  
        /// 资源名称  
        /// </summary>  
        public string ResName { get; set; }

        /// <summary>  
        /// 资源类型ID  
        /// </summary>  
        public long ResTypeId { get; set; }

        /// <summary>  
        /// 资源类型  
        /// </summary>  
        public string ResType { get; set; }

        /// <summary>  
        /// 资源类型名称  
        /// </summary>  
        public string ResTypeName { get; set; }

        /// <summary>  
        /// 工序ID  
        /// </summary>  
        public long ProcedureId { get; set; }

        /// <summary>  
        /// 工序代码  
        /// </summary>  
        public string ProcedureCode { get; set; }

        /// <summary>  
        /// 工序名称  
        /// </summary>  
        public string ProcedureName { get; set; }

        /// <summary>  
        /// 生产线ID  
        /// </summary>  
        public long LineId { get; set; }

        /// <summary>  
        /// 生产线工作中心代码  
        /// </summary>  
        public string LineWorkCenterCode { get; set; }

        /// <summary>  
        /// 生产线工作中心名称  
        /// </summary>  
        public string LineWorkCenterName { get; set; }

        /// <summary>  
        /// 车间ID  
        /// </summary>  
        public long WorkShopId { get; set; }

        /// <summary>  
        /// 车间代码  
        /// </summary>  
        public string WorkShopCode { get; set; }

        /// <summary>  
        /// 车间名称  
        /// </summary>  
        public string WorkShopName { get; set; }
    }
}
