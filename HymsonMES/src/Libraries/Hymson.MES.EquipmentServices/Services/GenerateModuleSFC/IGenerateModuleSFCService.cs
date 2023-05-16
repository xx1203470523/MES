using Hymson.MES.EquipmentServices.Request.GenerateModuleSFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.GenerateModuleSFC
{
    /// <summary>
    /// 请求生成模组码-电芯堆叠
    /// </summary>
    public interface IGenerateModuleSFCService
    {
        /// <summary>
        /// 请求生成模组码-电芯堆叠
        /// </summary>
        /// <param name="generateModuleSFCRequest"></param>
        /// <returns></returns>
        Task GenerateModuleSFCAsync(GenerateModuleSFCRequest generateModuleSFCRequest);
    }
}
