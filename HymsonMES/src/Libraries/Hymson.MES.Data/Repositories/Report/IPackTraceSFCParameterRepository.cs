﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public interface IPackTraceSFCParameterRepository
{
    public Task<IEnumerable<PackTraceSFCParameterView>> GetListAsync(PackTraceSFCParameterQuery query);

}