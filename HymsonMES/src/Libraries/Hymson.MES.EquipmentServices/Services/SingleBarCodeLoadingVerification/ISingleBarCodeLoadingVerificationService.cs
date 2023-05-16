using Hymson.MES.EquipmentServices.Request.SingleBarCodeLoadingVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification
{
    /// <summary>
    /// 单体条码上料校验
    /// </summary>
    public interface ISingleBarCodeLoadingVerificationService
    {
        /// <summary>
        /// 单体条码上料校验
        /// </summary>
        /// <param name="singleBarCodeLoadingVerificationRequest"></param>
        /// <returns></returns> 
        Task SingleBarCodeLoadingVerificationAsync(SingleBarCodeLoadingVerificationRequest singleBarCodeLoadingVerificationRequest);
    }
}
