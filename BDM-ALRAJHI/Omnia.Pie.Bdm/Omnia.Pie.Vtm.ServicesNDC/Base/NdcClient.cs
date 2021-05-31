using System;
using System.Threading.Tasks;
using System.Net.Security;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Channels;
using DotNetty.Handlers.Tls;
using DotNetty.Codecs;
using System.Security.Cryptography.X509Certificates;
using Omnia.Pie.Vtm.Framework.Extensions;
using Omnia.Pie.Vtm.Framework.Interface;
using System.Configuration;
using System.Net;
using System.Text;
using Omnia.Pie.Vtm.ServicesNdc.Interface;
using DotNetty.Buffers;

namespace Omnia.Pie.Vtm.ServicesNdc.Base
{
    public class NdcClient : INdcClient
    {
        private readonly ILogger _logger;
        private MultithreadEventLoopGroup group;
        private IChannel bootstrapChannel;
        private Bootstrap bootstrap;

        private INdcClientHandler ClientHandler { get; set; }
        private string Ip { get; set; }
        private int Port { get; set; }
        private int NdcRequestTimeOut { get; set; }

        public string CertificateLocation { get; set; }

        public string MessageTerminator { get { return "\r\n"; } }

        public string LastMessage { get; set; }

        private bool LogReqResp { get; set; }

        public NdcClient(IResolver container)
        {
            _logger = container.Resolve<ILogger>();

            _logger?.Info($"{GetType()} => Instantiating NdcClientHandler.");
            

            Ip = ConfigurationManager.AppSettings["NdcIp"].ToString();

            if (string.IsNullOrEmpty(Ip))
                throw new ArgumentNullException($"Missing Ndc {nameof(Ip)} in the config file.");

            CertificateLocation = ConfigurationManager.AppSettings["CertificateLocation"].ToString();

            if (string.IsNullOrEmpty(CertificateLocation))
                throw new ArgumentNullException($"Missing Ndc {nameof(CertificateLocation)} in the config file.");

            int.TryParse(ConfigurationManager.AppSettings["NdcPort"].ToString(), out var port);
            Port = port;

            var timeout = 20000;
            int.TryParse(ConfigurationManager.AppSettings["NDCRequestTimeOut"].ToString(), out timeout);
            NdcRequestTimeOut = timeout;

            _logger?.Info($"{GetType()} => Initialized.");

            LogReqResp = (ConfigurationManager.AppSettings["AppState"].ToString() == "TraceState" ? true : false);

        }
        
        public bool IsConnected()
        {
            if (bootstrapChannel != null)
                return bootstrapChannel.Active;
            else
                return false;

        }


        public async Task StartClientAsync()
        {
            if (bootstrapChannel != null)
            {
                await bootstrapChannel.CloseAsync();
            }

            ClientHandler = new NdcClientHandler(_logger);
            ClientHandler.NdcClient = this;

            group = new MultithreadEventLoopGroup();
            bootstrap = new Bootstrap();
            var serverCertificate = new X509Certificate2(CertificateLocation);
            string targetHost = serverCertificate.GetNameInfo(X509NameType.DnsName, false);

            _logger?.Info($"{GetType()} => Preparing Socket bootstrap.");

            bootstrap
                .Group(group)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.AutoRead, true)
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.SoKeepalive, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    
                    if (serverCertificate != null)
                    {
                        pipeline.AddLast(new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                    }

                    
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(Int32.MaxValue, 0, 2, 0, 0));

                    //pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                    //pipeline.AddLast(new StringEncoder());
                    //pipeline.AddLast(new StringDecoder());

                    pipeline.AddLast(ClientHandler);
                }));

            _logger?.Info($"{GetType()} => Connecting Socket.");
            bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(Ip), Port));
        }

        public async Task<string> SendAsync(byte[] msg, bool waitForResponse)
        {
            LastMessage = "";

            if (LogReqResp)
                _logger?.Info($"NDC Client : Request => {Encoding.Default.GetString(msg)}");

            _logger?.Info($"Sending Request MSG Length = > {Encoding.Default.GetString(msg).Length}");

            if (!IsConnected())
            {
                await StartClientAsync();
            }

            try
            {
                IByteBuffer buffer = Unpooled.WrappedBuffer(msg);
                await bootstrapChannel.WriteAndFlushAsync(buffer);
            }
            catch (Exception ex)
            {
                _logger?.Exception(ex);
                throw new Exception("NDC Service Failed");
            }

            
            if (waitForResponse)
            {
                _logger?.Info($"{GetType()} => Will wait for the response.");

                int counter = 0;
                do
                {
                    await Task.Delay(1000);
                    counter++;
                }

                while (LastMessage == "" && counter < 40);
                
            }

            if (LogReqResp)
                _logger?.Info($"NDC Client : Response =>  {LastMessage}");


            return LastMessage;
        }


        public void Dispose()
        {
            if (bootstrapChannel != null)
                bootstrapChannel.CloseAsync().Wait(2000);

            if (group != null)
                group.ShutdownGracefullyAsync().Wait(2000);
        }

        public async Task SendKeepAliveReadyBMsg()
        {
            await SendAsync(Encoding.UTF8.GetBytes(Convert.ToChar(0x016).ToString()), false);
        }

        public byte[] AppendLength2Bytes(byte[] data)
        {
            var len = data.Length;
            var buf = new byte[len + 2];
            buf[0] = (byte)(len >> 8 & 255);
            buf[1] = (byte)(len & 255);
            Array.Copy(data, 0, buf, 2, len);

            return buf;
        }

        public byte[] ObjectToByteArray(Object obj)
        {
            return Encoding.UTF8.GetBytes(obj.ToString());
        }

        /*
        private string GetReadyRequest()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("2", "Message Class");
            stringBuilder.Append("2", "Message Sub-Class");
            stringBuilder.Append("\u001c", "Field Separator");
            stringBuilder.Append("000", "LUNO");
            stringBuilder.Append("\u001c", "Field Separator");
            stringBuilder.Append("\u001c", "Field Separator");
            stringBuilder.Append("B", "data");
            return stringBuilder.ToStringExtend();
        }
        */
    }
}
