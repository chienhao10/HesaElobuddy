using ProtoBuf;

namespace ezBot_Shared
{
    [ProtoContract]
    public class NetworkMessage {
        [ProtoMember(1)]
        public byte Tag;
        [ProtoMember(2)]
        public byte[] Data;
    }
}