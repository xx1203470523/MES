/*
 *creator: Karl
 *
 *describe: 编码规则 页面视图类 
 *builder:  Karl
 *build datetime: 2023-03-24 14:28:17
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Core.Domain.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则 页面视图
    /// </summary>
    public class InteVehicleView : InteVehicleEntity
    {
        /// <summary>
        /// 载具类型编码
        /// </summary>
        public string VehicleTypeCode { get; set; }

        /// <summary>
        /// 载具类型名称
        /// </summary>
        public string VehicleTypeName { get; set; }
    }

    public class InteVehicleAboutVehicleTypeView : InteVehicleEntity
    {
        /// <summary>
        /// 类型编码
        /// </summary>
        public string VehicleTypeCode { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string VehicleTypeName { get; set; }

        /// <summary>
        /// 状态;0-未启用 1-启用
        /// </summary>
        public DisableOrEnableEnum VehicleTypeStatus { get; set; }

        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 单元数量
        /// </summary>
        public int CellQty { get; set; }
    }
}
