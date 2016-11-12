// Decompiled with JetBrains decompiler
// Type: RtmpSharp.Net.RtmpProxySource
// Assembly: rtmp-sharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8588136F-A4B9-4004-9712-4EA13AA4AF9D
// Assembly location: C:\Users\Hesa\Desktop\eZ_Source\bin\Debug\rtmp-sharp.dll

using Complete;
using Complete.Threading;
using RtmpSharp.IO;
using RtmpSharp.Messaging;
using RtmpSharp.Messaging.Events;
using RtmpSharp.Messaging.Messages;
using System;
using System.IO;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace RtmpSharp.Net
{
  internal class RtmpProxySource
  {
    private readonly RemoteCertificateValidationCallback _certificateValidator = (RemoteCertificateValidationCallback) ((sender, certificate, chain, errors) => true);
    public bool NoDelay = true;
    private readonly TaskCallbackManager<int, object> _callbackManager;
    private readonly ObjectEncoding _objectEncoding;
    private readonly SerializationContext _serializationContext;
    private int _invokeId;
    private RtmpPacketReader _reader;
    private Thread _readerThread;
    private RtmpPacketWriter _writer;
    private Thread _writerThread;

    public bool IsDisconnected { get; set; }

    public event EventHandler Disconnected;

    internal event EventHandler<RemotingMessageReceivedEventArgs> RemotingMessageReceived;

    internal event EventHandler<CommandMessageReceivedEventArgs> CommandMessageReceived;

    internal event EventHandler<ConnectMessageEventArgs> ConnectMessageReceived;

    public event EventHandler<Exception> CallbackException;

    public RtmpProxySource(SerializationContext serializationContext, Stream stream)
      : this(serializationContext)
    {
      this.DoHandshake(stream);
      this.EstablishThreads(stream);
      this._objectEncoding = ObjectEncoding.Amf3;
    }

    public RtmpProxySource(SerializationContext serializationContext)
    {
      if (serializationContext == null)
        throw new ArgumentNullException("serializationContext");
      this._serializationContext = serializationContext;
      this._callbackManager = new TaskCallbackManager<int, object>();
    }

    private void DoHandshake(Stream stream)
    {
      RtmpHandshake.Read(stream, true);
      Random random = new Random();
      byte[] numArray = new byte[1528];
      byte[] buffer = numArray;
      random.NextBytes(buffer);
      RtmpHandshake h = new RtmpHandshake() { Version = 3, Time = (uint) Environment.TickCount, Time2 = 0, Random = numArray };
      RtmpHandshake h2 = h.Clone();
      h2.Time2 = (uint) Environment.TickCount;
      RtmpHandshake.WriteAsync(stream, h, h2, true);
      RtmpHandshake.Read(stream, false);
    }

    private Task<object> QueueCommandAsTask(Command command, int streamId, int messageStreamId)
    {
      if (this.IsDisconnected)
        return RtmpProxySource.CreateExceptedTask((Exception) new ClientDisconnectedException("disconnected"));
      Task<object> task = this._callbackManager.Create(command.InvokeId);
      this._writer.Queue((RtmpEvent) command, streamId, messageStreamId);
      return task;
    }

    public void EstablishThreads(Stream stream)
    {
      this._writer = new RtmpPacketWriter(new AmfWriter(stream, this._serializationContext), ObjectEncoding.Amf3);
      this._reader = new RtmpPacketReader(new AmfReader(stream, this._serializationContext));
      this._reader.EventReceived += new EventHandler<EventReceivedEventArgs>(this.EventReceivedCallback);
      this._reader.Disconnected += new EventHandler<ExceptionalEventArgs>(this.OnPacketProcessorDisconnected);
      this._writer.Disconnected += new EventHandler<ExceptionalEventArgs>(this.OnPacketProcessorDisconnected);
      this._writerThread = new Thread(new ThreadStart(this._reader.ReadLoop))
      {
        IsBackground = true
      };
      this._readerThread = new Thread(new ThreadStart(this._writer.WriteLoop))
      {
        IsBackground = true
      };
      this._writerThread.Start();
      this._readerThread.Start();
    }

    private void OnPacketProcessorDisconnected(object sender, ExceptionalEventArgs e)
    {
      this.OnDisconnect(e);
    }

    private void OnDisconnect(ExceptionalEventArgs e)
    {
      if (this.IsDisconnected)
        return;
      this.IsDisconnected = true;
      if (this._writer != null)
        this._writer.Continue = false;
      if (this._reader != null)
        this._reader.Continue = false;
      this.WrapCallback((Action) (() => this._callbackManager.SetExceptionForAll((Exception) new ClientDisconnectedException(e.Description, e.Exception))));
      this._invokeId = 0;
      this.WrapCallback((Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        EventHandler disconnected = this.Disconnected;
        if (disconnected == null)
          return;
        ExceptionalEventArgs exceptionalEventArgs = e;
        disconnected((object) this, (EventArgs) exceptionalEventArgs);
      }));
    }

    public void Close()
    {
      this.OnDisconnect(new ExceptionalEventArgs("closed"));
    }

    private async void EventReceivedCallback(object sender, EventReceivedEventArgs e)
    {
      try
      {
        Command command;
        object param;
        switch (e.Event.MessageType)
        {
          case MessageType.UserControlMessage:
            UserControlMessage userControlMessage = (UserControlMessage) e.Event;
            if (userControlMessage.EventType == UserControlMessageType.PingRequest)
            {
              this.WriteProtocolControlMessage((RtmpEvent) new UserControlMessage(UserControlMessageType.PingResponse, userControlMessage.Values));
              break;
            }
            break;
          case MessageType.CommandAmf3:
          case MessageType.DataAmf0:
          case MessageType.CommandAmf0:
            command = (Command) e.Event;
            Method methodCall = command.MethodCall;
            param = methodCall.Parameters.Length == 1 ? methodCall.Parameters[0] : (object) methodCall.Parameters;
            if (methodCall.Name == "_result" || methodCall.Name == "_error" || methodCall.Name == "receive")
              throw new InvalidDataException();
            if (!(methodCall.Name == "onstatus"))
            {
              if (methodCall.Name == "connect")
              {
                CommandMessage parameter = (CommandMessage) methodCall.Parameters[3];
                object obj1;
                parameter.Headers.TryGetValue("DSEndpoint", out obj1);
                object obj2;
                parameter.Headers.TryGetValue("DSId", out obj2);
                ConnectMessageEventArgs args = new ConnectMessageEventArgs((string) methodCall.Parameters[1], (string) methodCall.Parameters[2], parameter, (string) obj1, (string) obj2, command.InvokeId, (AsObject) command.ConnectionParameters);
                // ISSUE: reference to a compiler-generated field
                EventHandler<ConnectMessageEventArgs> connectMessageReceived = this.ConnectMessageReceived;
                if (connectMessageReceived != null)
                {
                  ConnectMessageEventArgs e1 = args;
                  connectMessageReceived((object) this, e1);
                }
                if (parameter.Operation == CommandOperation.ClientPing)
                {
                  AsObject asObject1 = await this.InvokeConnectResultAsync(command.InvokeId, (AsObject) args.Result.Body);
                }
                else
                {
                  AsObject asObject2 = await this.InvokeReconnectResultInvokeAsync(command.InvokeId, (AsObject) args.Result.Body);
                }
                args = (ConnectMessageEventArgs) null;
                break;
              }
              if (param is RemotingMessage)
              {
                RemotingMessage message = param as RemotingMessage;
                object obj1;
                message.Headers.TryGetValue("DSEndpoint", out obj1);
                object obj2;
                message.Headers.TryGetValue("DSId", out obj2);
                string endpoint = (string) obj1;
                string clientId = (string) obj2;
                int invokeId = command.InvokeId;
                RemotingMessageReceivedEventArgs receivedEventArgs = new RemotingMessageReceivedEventArgs(message, endpoint, clientId, invokeId);
                // ISSUE: reference to a compiler-generated field
                EventHandler<RemotingMessageReceivedEventArgs> remotingMessageReceived = this.RemotingMessageReceived;
                if (remotingMessageReceived != null)
                {
                  RemotingMessageReceivedEventArgs e1 = receivedEventArgs;
                  remotingMessageReceived((object) this, e1);
                }
                if (receivedEventArgs.Error == null)
                {
                  this.InvokeResult(command.InvokeId, receivedEventArgs.Result);
                  break;
                }
                this.InvokeError(command.InvokeId, receivedEventArgs.Error);
                break;
              }
              if (param is CommandMessage)
              {
                CommandMessage message = param as CommandMessage;
                object obj1;
                message.Headers.TryGetValue("DSEndpoint", out obj1);
                object obj2;
                message.Headers.TryGetValue("DSId", out obj2);
                string endpoint = obj1 as string;
                string dsId = obj2 as string;
                int invokeId = command.InvokeId;
                CommandMessageReceivedEventArgs receivedEventArgs = new CommandMessageReceivedEventArgs(message, endpoint, dsId, invokeId);
                // ISSUE: reference to a compiler-generated field
                EventHandler<CommandMessageReceivedEventArgs> commandMessageReceived = this.CommandMessageReceived;
                if (commandMessageReceived != null)
                {
                  CommandMessageReceivedEventArgs e1 = receivedEventArgs;
                  commandMessageReceived((object) this, e1);
                }
                this.InvokeResult(command.InvokeId, receivedEventArgs.Result);
                break;
              }
              break;
            }
            break;
        }
        command = (Command) null;
        param = (object) null;
      }
      catch (ClientDisconnectedException ex)
      {
      }
    }

    internal void InvokeResult(int invokeId, AcknowledgeMessageExt message)
    {
      if (this._objectEncoding != ObjectEncoding.Amf3)
        throw new NotSupportedException("Flex RPC requires AMF3 encoding.");
      InvokeAmf3 invokeAmf3 = new InvokeAmf3();
      invokeAmf3.InvokeId = invokeId;
      invokeAmf3.MethodCall = new Method("_result", new object[1]
      {
        (object) message
      }, 1 != 0, CallStatus.Result);
      this.QueueCommandAsTask((Command) invokeAmf3, 3, 0);
    }

    internal void InvokeError(int invokeId, ErrorMessage message)
    {
      if (this._objectEncoding != ObjectEncoding.Amf3)
        throw new NotSupportedException("Flex RPC requires AMF3 encoding.");
      InvokeAmf3 invokeAmf3 = new InvokeAmf3();
      invokeAmf3.InvokeId = invokeId;
      invokeAmf3.MethodCall = new Method("_error", new object[1]
      {
        (object) message
      }, 0 != 0, CallStatus.Result);
      this.QueueCommandAsTask((Command) invokeAmf3, 3, 0);
    }

    public void InvokeError(int invokeId, string correlationId, object rootCause, string faultDetail, string faultString, string faultCode)
    {
      ErrorMessage errorMessage = new ErrorMessage();
      string str1 = Uuid.NewUuid();
      errorMessage.ClientId = str1;
      string str2 = Uuid.NewUuid();
      errorMessage.MessageId = str2;
      string str3 = correlationId;
      errorMessage.CorrelationId = (object) str3;
      object obj = rootCause;
      errorMessage.RootCause = obj;
      ErrorMessage message = errorMessage;
      this.InvokeError(invokeId, message);
    }

    internal void InvokeReceive(string clientId, string subtopic, object body)
    {
      InvokeAmf3 invokeAmf3_1 = new InvokeAmf3();
      invokeAmf3_1.InvokeId = 0;
      InvokeAmf3 invokeAmf3_2 = invokeAmf3_1;
      string methodName = "receive";
      object[] parameters = new object[1];
      int index = 0;
      AsyncMessageExt asyncMessageExt = new AsyncMessageExt();
      asyncMessageExt.Headers = new AsObject()
      {
        {
          "DSSubtopic",
          (object) subtopic
        }
      };
      string str1 = clientId;
      asyncMessageExt.ClientId = str1;
      object obj = body;
      asyncMessageExt.Body = obj;
      string str2 = Uuid.NewUuid();
      asyncMessageExt.MessageId = str2;
      parameters[index] = (object) asyncMessageExt;
      int num1 = 1;
      int num2 = 0;
      Method method = new Method(methodName, parameters, num1 != 0, (CallStatus) num2);
      invokeAmf3_2.MethodCall = method;
      this.QueueCommandAsTask((Command) invokeAmf3_1, 3, 0);
    }

    public async Task<AsObject> ConnectResultInvokeAsync(object[] parameters)
    {
      this.WriteProtocolControlMessage((RtmpEvent) new WindowAcknowledgementSize(245248000));
      this.WriteProtocolControlMessage((RtmpEvent) new PeerBandwidth(250000, (byte) 2));
      this.SetChunkSize(50000);
      InvokeAmf0 invokeAmf0 = new InvokeAmf0();
      invokeAmf0.MethodCall = new Method("_result", new object[1]
      {
        (object) new AsObject()
        {
          {
            "objectEncoding",
            (object) 3.0
          },
          {
            "level",
            (object) "status"
          },
          {
            "details",
            (object) null
          },
          {
            "description",
            (object) "Connection succeeded."
          },
          {
            "DSMessagingVersion",
            (object) 1.0
          },
          {
            "code",
            (object) "NetConnection.Connect.Success"
          },
          {
            "id",
            (object) Uuid.NewUuid()
          }
        }
      }, 1 != 0, CallStatus.Request);
      invokeAmf0.InvokeId = this.GetNextInvokeId();
      return (AsObject) await this.QueueCommandAsTask((Command) invokeAmf0, 3, 0);
    }

    public async Task<AsObject> InvokeConnectResultAsync(int invokeId, AsObject param)
    {
      this.WriteProtocolControlMessage((RtmpEvent) new WindowAcknowledgementSize(245248000));
      this.WriteProtocolControlMessage((RtmpEvent) new PeerBandwidth(250000, (byte) 2));
      this.SetChunkSize(50000);
      InvokeAmf0 invokeAmf0 = new InvokeAmf0();
      invokeAmf0.MethodCall = new Method("_result", new object[1]
      {
        (object) param
      }, 1 != 0, CallStatus.Request);
      invokeAmf0.InvokeId = invokeId;
      return (AsObject) await this.QueueCommandAsTask((Command) invokeAmf0, 3, 0);
    }

    public async Task<AsObject> InvokeReconnectResultInvokeAsync(int invokeId, AsObject param)
    {
      this.SetChunkSize(50000);
      InvokeAmf0 invokeAmf0 = new InvokeAmf0();
      invokeAmf0.MethodCall = new Method("_result", new object[1]
      {
        (object) param
      }, 1 != 0, CallStatus.Request);
      invokeAmf0.InvokeId = invokeId;
      return (AsObject) await this.QueueCommandAsTask((Command) invokeAmf0, 3, 0);
    }

    public void SetChunkSize(int size)
    {
      this.WriteProtocolControlMessage((RtmpEvent) new ChunkSize(size));
    }

    private void WriteProtocolControlMessage(RtmpEvent @event)
    {
      this._writer.Queue(@event, 2, 0);
    }

    private int GetNextInvokeId()
    {
      return Interlocked.Increment(ref this._invokeId);
    }

    private void WrapCallback(Action action)
    {
      try
      {
        try
        {
          action();
        }
        catch (Exception ex)
        {
          // ISSUE: reference to a compiler-generated field
          EventHandler<Exception> callbackException = this.CallbackException;
          if (callbackException == null)
            return;
          Exception e = ex;
          callbackException((object) this, e);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private static Task<object> CreateExceptedTask(Exception exception)
    {
      TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
      Exception exception1 = exception;
      completionSource.SetException(exception1);
      return completionSource.Task;
    }
  }
}
