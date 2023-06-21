using Hymson.MES.CoreServices.Bos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.NewJob.Correlation
{
    internal class CorrelationJobService : ICorrelationJobService
    {
        public CorrelationJobService() { }

        /// <summary>
        /// 获取job数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JobBo> GetJob()
        {
            throw new NotImplementedException();
        }
    }
}
