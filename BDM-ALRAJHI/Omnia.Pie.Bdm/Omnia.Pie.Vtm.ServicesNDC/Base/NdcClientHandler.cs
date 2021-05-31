using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.ServicesNdc.Interface;
using System;
using System.Text;
using System.Timers;

namespace Omnia.Pie.Vtm.ServicesNdc.Base
{
    public class NdcClientHandler : ChannelHandlerAdapter, INdcClientHandler
    {
        private readonly ILogger _logger;
        //private Timer _keepAlive;

        public INdcClient NdcClient { get; set; }

        public NdcClientHandler(ILogger logger)
        {
            _logger = logger;

            //_logger?.Info($"{GetType()} => Resolving INdcClient.");
            //NdcClient = container.Resolve<INdcClient>();
        }

        public override void ChannelRead(IChannelHandlerContext context, object msg)
        {
            string message = "";

            /*
            if (_keepAlive.Enabled)
                _keepAlive.Stop();
                */


            var byteBuffer = msg as IByteBuffer;
            if (byteBuffer != null)
            {
                message = byteBuffer.ToString(Encoding.UTF8);
                _logger?.Info($"Received Response Length = > {message.Length}");

            }
            
            
            if (message == null)
            {
                _logger?.Info($"Response Message is null");
                return;
            }

            _logger?.Info($"Passing msg value to NdcClient.LastMessage");
            NdcClient.LastMessage = message;

            context.Flush();

            //_keepAlive.Start();
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            _logger.Error(string.Format("Error Caught at {0}", DateTime.Now.Millisecond));
            _logger.Error(string.Format("Error Trace: {0}", e.StackTrace));
            contex.CloseAsync();
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _logger?.Info("NDC Connection Inactive!");

            /*
            if (_keepAlive.Enabled)
                _keepAlive.Stop();
            */
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _logger?.Info("NDC Connection Active!");
            
            /*
            if (_keepAlive == null)
            {
                _keepAlive = new Timer();
                _keepAlive.Elapsed += new ElapsedEventHandler(ForcedHeartBeat);
                _keepAlive.Interval = (double)240000; // 4 minutes
                _keepAlive.AutoReset = true;
                _keepAlive.Start();
            }
            */
        }


        private async void ForcedHeartBeat(object sender, EventArgs e)
        {
            if (NdcClient.IsConnected())
            {
                await NdcClient.SendKeepAliveReadyBMsg();
            }
            else
            {
                _logger?.Info("Not connected to NDC server...");
            }
        }

    }
}
        