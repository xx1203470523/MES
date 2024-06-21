﻿using Hymson.MES.SystemServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Services
{
    /// <summary>
    /// 生产领料
    /// </summary>
    public interface IManuRequistionOrderService
    {
        /// <summary>
        /// 生产领料
        /// </summary>
        /// <param name="productionPickDto"></param>
        /// <returns></returns>
        Task SavePickMaterialsAsync(ProductionPickDto productionPickDto);
    }
}
