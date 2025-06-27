using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.NxC.Interfaces
{
    public interface IMessageBatchService
    {
       Task UpdateMsgStateAsync(CancellationToken cancellationToken);
    }
}
