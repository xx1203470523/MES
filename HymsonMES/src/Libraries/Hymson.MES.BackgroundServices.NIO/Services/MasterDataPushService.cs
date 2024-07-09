using Hymson.MES.BackgroundServices.NIO.Dtos;
using Hymson.MES.BackgroundServices.NIO.Dtos.Master;
using RestSharp;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（主数据）
    /// </summary>
    public class MasterDataPushService : BasePushService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MasterDataPushService() { }

        /// <summary>
        /// 主数据（产品）
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static async Task<RestResponse> ProductAsync(IEnumerable<ProductDto> dtos)
        {
            var bodyDto = new ClientRequestDto<ProductDto>
            {
                SchemaCode = "",
                List = dtos
            };

            return await ExecuteAsync("/v1/trans/masterdata/product", bodyDto);
        }

        /// <summary>
        /// 主数据（工站）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> StationAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/masterdata/station", jsonBody);
        }

        /// <summary>
        /// 主数据（控制项）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> FieldAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/masterdata/field", jsonBody);
        }

        /// <summary>
        /// 主数据（一次合格率目标）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> PassrateTargetAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/masterdata/passrate_target", jsonBody);
        }

        /// <summary>
        /// 主数据（环境监测）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> EnvFieldAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/masterdata/envfield", jsonBody);
        }

        /// <summary>
        /// 主数据（人员资质）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> PersonCertAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/masterdata/personcert", jsonBody);
        }

        /// <summary>
        /// 主数据（排班）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> TeamSchedulingAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/masterdata/teamscheduling", jsonBody);
        }

    }
}
