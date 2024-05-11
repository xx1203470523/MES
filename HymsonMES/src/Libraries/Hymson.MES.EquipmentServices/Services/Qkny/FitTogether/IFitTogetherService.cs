using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.FitTogether
{
    /// <summary>
    /// 装配
    /// </summary>
    public interface IFitTogetherService
    {
        /// <summary>
        /// 多极组产品出站031
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<OutboundMoreReturnDto>> OutboundMultPolarAsync(OutboundMultPolarDto dto);

        /// <summary>
        /// 绑定后极组单个条码进站049
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task InboundBindJzSingleAsync(InboundBindJzSingleDto dto);

        /// <summary>
        /// 电芯码下发033
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<string>> GenerateCellSfcAsync(GenerateCellSfcDto dto);

        /// <summary>
        /// 电芯极组绑定产品出站032
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OutboundSfcPolarAsync(OutboundSfcPolarDto dto);

        /// <summary>
        /// 绑定后极组单个条码出站050
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OutboundBindJzSingleAsync(OutboundBindJzSingleDto dto);

        /// <summary>
        /// 卷绕极组产出上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CollingPolarAsync(CollingPolarDto dto);

        /// <summary>
        /// 生成24位国标电芯码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<string> Create24GbCodeAsync(GenerateDxSfcDto dto);
    }
}
