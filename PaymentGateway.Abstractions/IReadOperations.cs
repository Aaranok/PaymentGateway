using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Abstractions
{
    public interface IReadOperation<TInput, TResult>
    {
        TResult PerformOperation(TInput query);
    }
}
