using Hymson.MES.Core.Domain.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment
{
    /// <summary>
    /// 设备注册 View
    /// </summary>
    public class EquEquipmentPageView: EquEquipmentEntity
    {
        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkCenterShopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkCenterShopName { get; set; }

    }

    /// <summary>
    /// 设备注册 View
    /// </summary>
    public class GetEquSpotcheckPlanEquipmentRelationPageView : EquEquipmentEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 工作中心编码 
        /// </summary>
        public string WorkCenterCode { get; set; }

        /// <summary>
        /// 设备组
        /// </summary>
        public string EquipmentGroupCode { get; set; }
        /// <summary>
        /// 模板
        /// </summary>
        public long TemplateId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateCode { get; set; }
        /// <summary>
        /// 模板版本
        /// </summary>
        public string TemplateVersion { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string ExecutorIds { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public string LeaderIds { get; set; }

    }
}
