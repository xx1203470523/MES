using Hymson.MES.CoreServices.Bos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Job.JobUtility.Correlation
{
    public interface ICorrelationJobService
    {
        Task<JobBo> GetJob();
    }
}
