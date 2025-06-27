using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Database.Enum
{
    public enum MessageEventCode
    {
        Init,
        Receive,
        Reject,
        Ack,
        Nak,
        Expire,
    }
}
