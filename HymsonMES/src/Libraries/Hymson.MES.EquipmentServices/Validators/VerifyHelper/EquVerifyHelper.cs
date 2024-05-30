using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ToolBindMaterial;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.EquVerifyHelper
{
    /// <summary>
    /// 设备接口入参校验
    /// </summary>
    public static class EquVerifyHelper
    {
        /// <summary>
        /// 通用校验
        /// </summary>
        /// <param name="dto"></param>
        private static void Common(QknyBaseDto dto)
        {

        }

        /// <summary>
        /// 操作员登录001
        /// </summary>
        /// <param name="dto"></param>
        public static void OperationLoginDto(OperationLoginDto dto)
        {
            Common(dto);
            if(string.IsNullOrEmpty(dto.OperatorUserID) == true || string.IsNullOrEmpty(dto.OperatorPassword) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19153));
            }
        }

        /// <summary>
        /// 设备状态上报003
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void StateDto(StateDto dto)
        {
            Common(dto);
            List<string> statusList = new List<string>() { "1", "2", "3" };
            if(statusList.Contains(dto.StateCode) == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19154));
            }
        }

        /// <summary>
        /// 获取开机参数明细008
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void GetRecipeDetailDto(GetRecipeDetailDto dto)
        {
            Common(dto);
            if (string.IsNullOrEmpty(dto.RecipeCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19155));
            }
        }

        /// <summary>
        /// 开机参数校验采集009
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void RecipeDto(RecipeDto dto)
        {
            Common(dto);
            if(string.IsNullOrEmpty (dto.RecipeCode) == true 
                && string.IsNullOrEmpty(dto.Version) 
                && string.IsNullOrEmpty(dto.ProductCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19156));
            }
            if(dto.ParamList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19157));
            }
        }

        /// <summary>
        /// 原材料上料010
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void FeedingDto(FeedingDto dto)
        {
            Common(dto);
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 半成品上料011
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void HalfFeedingDto(HalfFeedingDto dto)
        {
            Common(dto);
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 产出上报024
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundMetersReportDto(OutboundMetersReportDto dto)
        {
            Common(dto);
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
            if(dto.TotalQty <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19158));
            }
            if(dto.TotalQty != (dto.OkQty + dto.NgQty))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19159));
            }
            List<string> typeList = new List<string>() { "1", "2", "3", "4", "5" };
            if(typeList.Contains(dto.OutputType) == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19160));
            }
        }

        /// <summary>
        /// 进站参数校验
        /// </summary>
        /// <param name="dto"></param>
        public static void InboundDto(InboundDto dto)
        {
            if(string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 出站参数校验
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundDto(OutboundDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 多个进站参数校验
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void InboundMoreDto(InboundMoreDto dto)
        {
            if(dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
        }

        /// <summary>
        /// 多个出站参数校验
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundMoreDto(OutboundMoreDto dto)
        {
            if (dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
        }

        /// <summary>
        /// 产品参数上传
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void ProductParamDto(ProductParamDto dto)
        {
            if(dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
        }

        /// <summary>
        /// 库存接收047
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void MaterialInventoryDto(MaterialInventoryDto dto)
        {
            if(dto.BarCodeList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
        }

        /// <summary>
        /// 多极组产品出站031
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundMultPolarDto(OutboundMultPolarDto dto)
        {
            if(dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
        }

        /// <summary>
        /// 电芯极组绑定产品出站032
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundSfcPolarDto(OutboundSfcPolarDto dto)
        {
            if(string.IsNullOrEmpty(dto.JzSfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 电芯码下发033
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void GenerateDxSfcDto(GenerateDxSfcDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 空托盘校验035
        /// </summary>
        /// <param name="dto"></param>
        public static void EmptyContainerCheckDto(EmptyContainerCheckDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }
        }

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void ContainerSfcCheckDto(ContainerSfcCheckDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 托盘电芯绑定(在制品容器)037
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void BindContainerDto(BindContainerDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }

            if(dto.ContainerSfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19103));
            }
        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void UnBindContainerDto(UnBindContainerDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }

            if (dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19103));
            }
        }

        /// <summary>
        /// 托盘NG电芯上报039
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void ContainerNgReportDto(ContainerNgReportDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }

            if (dto.NgSfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19103));
            }
        }

        /// <summary>
        /// 托盘进站(容器进站)040
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void InboundInContainerDto(InboundInContainerDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }
        }

        /// <summary>
        /// 托盘出站(容器出站)041
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundInContainerDto(OutboundInContainerDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }
        }

        /// <summary>
        /// 工装寿命上报042
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void ToolLifeDto(ToolLifeDto dto)
        {
            if (dto.ToolLifes.IsNullOrEmpty() == true && string.IsNullOrEmpty(dto.ToolCode) == true) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19151));
            }
        }

        /// <summary>
        /// 卷绕极组产出044
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void CollingPolarDto(CollingPolarDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 分选规则045
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void SortingRuleDto(SortingRuleDto dto)
        {
            if(string.IsNullOrEmpty(dto.Sfc) == true && string.IsNullOrEmpty(dto.ProductCode) == true) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19152));
            }
        }

        /// <summary>
        /// 产品参数上传046
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void ProductParamSameMultSfcDto(ProductParamSameMultSfcDto dto)
        {
            if (dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }

            if (dto.ParamList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19110));
            }
        }

        /// <summary>
        /// 工装条码绑定048
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void ToolBindMaterialDto(ToolBindMaterialDto dto) 
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }

            if (dto.ContainerSfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19103));
            }
        }

        /// <summary>
        /// 绑定后极组单个条码进站049
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void InboundBindJzSingleDto(InboundBindJzSingleDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 绑定后极组单个条码出站050
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void OutboundBindJzSingleDto(OutboundBindJzSingleDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

        /// <summary>
        /// 获取电芯信息051
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void GetSfcInfoDto(GetSfcInfoDto dto)
        {
            if (dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
        }

        /// <summary>
        /// 分选拆盘052
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void SortingUnBindDto(SortingUnBindDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }

            if (dto.SfcList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19103));
            }
        }

        /// <summary>
        /// 分选出站053
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void SortingOutboundDto(SortingOutboundDto dto)
        {
            if (string.IsNullOrEmpty(dto.ContainerCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19102));
            }
        }

        /// <summary>
        /// 设备文件上传054
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="CustomerValidationException"></exception>
        public static void EquFileUploadDto(EquFileUploadDto dto)
        {
            if (string.IsNullOrEmpty(dto.Sfc) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
        }

    }
}
