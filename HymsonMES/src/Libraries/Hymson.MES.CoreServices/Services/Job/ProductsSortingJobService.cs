using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 产品分选
    /// </summary>
    [Job("产品分选", JobTypeEnum.Standard)]
    public class ProductsSortingJobService : IJobService
    {
        private readonly IMasterDataService _masterDataService;
        /// <summary>
        /// 条码档位表 仓储
        /// </summary>
        private readonly IManuSfcGradeRepository _manuSfcGradeRepository;

        /// <summary>
        /// 条码档位明细表 仓储
        /// </summary>
        private readonly IManuSfcGradeDetailRepository _gradeDetailRepository;

        private readonly IManuSfcRepository _manuSfcRepository;

        public ProductsSortingJobService(IManuSfcRepository manuSfcRepository,IMasterDataService masterDataService,
            IManuSfcGradeRepository manuSfcGradeRepository,
            IManuSfcGradeDetailRepository gradeDetailRepository)
        {
            _manuSfcRepository = manuSfcRepository;
            _masterDataService = masterDataService;
            _manuSfcGradeRepository = manuSfcGradeRepository;
            _gradeDetailRepository = gradeDetailRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.InStationRequestBos == null || commonBo.InStationRequestBos.Any() == false) return;

            var sfcs = commonBo.InStationRequestBos.Select(s => s.SFC);
            // 验证DTO
            if (sfcs == null || sfcs.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            // 获取条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_manuSfcRepository.GetBySFCsAsync, sfcs);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16128)).WithData("SFC", string.Join(',', sfcs));
            }
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.InStationRequestBos == null || commonBo.InStationRequestBos.Any() == false) return default;

            var sfcs = commonBo.InStationRequestBos.Select(s => s.SFC);
            // 验证DTO
            if (sfcs == null || sfcs.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            // 获取条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_manuSfcRepository.GetBySFCsAsync, sfcs);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false)
            {
                return default;
            }
            //为了不报错
            await Task.CompletedTask;
            return default;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            //为了不报错
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}