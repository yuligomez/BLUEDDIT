using System;
namespace ProtocolComunication.Interface
{
    public interface IHeaderHandler
    {
        byte[] EncodeHeader(short command, int dataLength);
        Tuple<short, int> DecodeHeader(byte[] data);
    }
}
