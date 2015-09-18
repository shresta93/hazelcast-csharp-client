using Hazelcast.Client.Protocol;
using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using System.Collections.Generic;

namespace Hazelcast.Client.Protocol.Codec
{
    internal sealed class TransactionalListAddCodec
    {

        public static readonly TransactionalListMessageType RequestType = TransactionalListMessageType.TransactionalListAdd;
        public const int ResponseType = 101;
        public const bool Retryable = false;

        //************************ REQUEST *************************//

        public class RequestParameters
        {
            public static readonly TransactionalListMessageType TYPE = RequestType;
            public string name;
            public string txnId;
            public long threadId;
            public IData item;

            public static int CalculateDataSize(string name, string txnId, long threadId, IData item)
            {
                int dataSize = ClientMessage.HeaderSize;
                dataSize += ParameterUtil.CalculateDataSize(name);
                dataSize += ParameterUtil.CalculateDataSize(txnId);
                dataSize += Bits.LongSizeInBytes;
                dataSize += ParameterUtil.CalculateDataSize(item);
                return dataSize;
            }
        }

        public static ClientMessage EncodeRequest(string name, string txnId, long threadId, IData item)
        {
            int requiredDataSize = RequestParameters.CalculateDataSize(name, txnId, threadId, item);
            ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
            clientMessage.SetMessageType((int)RequestType);
            clientMessage.SetRetryable(Retryable);
            clientMessage.Set(name);
            clientMessage.Set(txnId);
            clientMessage.Set(threadId);
            clientMessage.Set(item);
            clientMessage.UpdateFrameLength();
            return clientMessage;
        }

        //************************ RESPONSE *************************//


        public class ResponseParameters
        {
            public bool response;
        }

        public static ResponseParameters DecodeResponse(IClientMessage clientMessage)
        {
            ResponseParameters parameters = new ResponseParameters();
            bool response ;
            response = clientMessage.GetBoolean();
            parameters.response = response;
            return parameters;
        }

    }
}
