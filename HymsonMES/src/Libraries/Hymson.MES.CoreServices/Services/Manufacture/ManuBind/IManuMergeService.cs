using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuBind
{
    public interface IManuMergeService
    {
        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        Task UnBindAsync(ManuUnBindDto param, ILocalizationService localizationService);
        /// <summary>
        /// 条码合并
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        Task<string> MergeAsync(ManuMergeRequestDto param,string updateName);
    }
}
