using DotNetty.Transport.Channels;
using Omnia.Pie.Vtm.ServicesNdc.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.ServicesNdc.Interface
{
    public interface INdcClientHandler : IChannelHandler
    {
        INdcClient NdcClient { get; set; }
    }
}
