using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuBind
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManuBindService
    {

        /// <summary>
        /// <summary>
        /// 条码绑定-活动
        /// </summary>
        /// <returns></returns>
        Task BindByActive(ManuBindDto param, ILocalizationService localizationService);

        /// <summary>
        /// 条码绑定-完成
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        Task BindByComplete(ManuBindDto param, ILocalizationService localizationService);

        /// <summary>
        /// 解除绑定
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        Task UnBind(ManuUnBindDto param, ILocalizationService localizationService);
    }
}
