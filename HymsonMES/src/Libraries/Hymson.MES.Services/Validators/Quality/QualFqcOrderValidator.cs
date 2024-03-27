using FluentValidation;
using Hymson.MES.Services.Dtos.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Validators.Quality
{
    internal class QualFqcOrderSaveValidator : AbstractValidator<QualFqcOrderSaveDto>
    {
        public QualFqcOrderSaveValidator() { }
    }
}
