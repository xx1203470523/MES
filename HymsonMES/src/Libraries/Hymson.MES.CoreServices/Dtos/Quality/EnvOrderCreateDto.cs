using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.QualEnvOrder;
using Hymson.MES.Core.Domain.QualEnvOrderDetail;
using Hymson.MES.Core.Domain.Quality;

namespace Hymson.MES.CoreServices.Dtos.Quality
{
    public class EnvOrderCreateReqDto
    {
        public long SiteId { get; set; }

        public string UserName { get; set; }

        public DateTime OperateTime { get; set; }

        public QualEnvParameterGroupEntity ParameterGroupEntity { get; set; }

        public IEnumerable<QualEnvParameterGroupDetailEntity> ParameterGroupDetails { get; set; }

        public IEnumerable<ProcParameterEntity> ParameterEntities { get; set; }

        public IEnumerable<PlanShiftDetailEntity> ShiftDetails { get; set; }
    }

    public class EnvOrderCreateRspDto
    {
        public QualEnvOrderEntity EnvOrderEntity { get; set; }

        public IEnumerable<QualEnvOrderDetailEntity> EnvOrderDetailEntities { get; set; }

        public QualEnvParameterGroupSnapshootEntity ParameterGroupSnapshootEntity { get; set; }

        public IEnumerable<QualEnvParameterGroupDetailSnapshootEntity> ParameterGroupDetailSnapshootEntities { get; set; }
    }
}
