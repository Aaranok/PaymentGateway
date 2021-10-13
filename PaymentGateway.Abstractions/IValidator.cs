﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.ReadOperations
{
    public interface IValidator<TInput>
    {
        bool Validate(TInput input);
    }
}
