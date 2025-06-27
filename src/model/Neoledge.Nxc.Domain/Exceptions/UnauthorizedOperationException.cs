using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Exceptions
{
    public class UnauthorizedOperationException : Exception
    {
        public UnauthorizedOperationException(string operation, string entity)
            : base($"You are not authorized to perform '{operation}' on {entity}.") { }
    }
}