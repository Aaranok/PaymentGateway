using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Abstractions
{
    public interface IWriteOperations<T>
    {
        public void PerformOperation(T operation);
    }
}
