/*
 *creator: Karl
 *
 *describe: 操作面板    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:56:57
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板-生产过站 service接口
    /// </summary>
    public class ManuFacePlateProductionService : IManuFacePlateProductionService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        #region Repository
        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        public ManuFacePlateProductionService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }
        #endregion

        #region 组装
        /// <summary>
        /// 组装界面获取当前条码对应bom下 当前需要组装的物料信息（操作面板）
        /// </summary>
        /// <returns></returns>
        public async Task Getss(string sfc, string procedureId)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(sfc);
            //判断工序是否一致
            if (manuSfcProduceEntity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES16901));
            }

            //获取对应bom下所有的物料(包含替代物料)

            //获取对应 条码流转表 里已经组装过的数据

            //处理之后的物料按顺序排布

        }

        #endregion

    }
}
