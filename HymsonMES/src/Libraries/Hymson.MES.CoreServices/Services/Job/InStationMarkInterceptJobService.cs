using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 
    /// </summary>
    [Job("NG标识拦截", JobTypeEnum.Standard)]
    public  class InStationMarkInterceptJobService : IJobService
    {
        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（不合格代码）
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        public InStationMarkInterceptJobService(IManuProductBadRecordRepository manuProductBadRecordRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository)
        {
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
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
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 获取不良记录
            var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = commonBo.SiteId,
                Sfcs = multiSFCBo.SFCs,
                Status = ProductBadRecordStatusEnum.Open
            });
            if (manuProductBadRecordEntities == null || !manuProductBadRecordEntities.Any()) return default;

            // 读取存在的不合格代码
            var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(manuProductBadRecordEntities.Select(x => x.UnqualifiedId));
            if (qualUnqualifiedCodeEntities == null || !qualUnqualifiedCodeEntities.Any()) return default;

            if (qualUnqualifiedCodeEntities.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Identification))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17108)).WithData("SFCs", string.Join(",", manuProductBadRecordEntities.Select(x => x.SFC).Distinct()));
            }

            return default;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return new JobResponseBo();
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
