using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BZFlag.Data.Utils;

namespace BZFlag.Networking.Messages.BZFS.Control
{
    public class MsgPause : NetworkMessage
    {
        public int PlayerID = -1;
        public bool Paused = false;

        public MsgPause()
        {
            Code = CodeFromChars("pa");
        }

        public override byte[] Pack()
        {
            DynamicOutputBuffer buffer = DynamicOutputBuffer.Get(Code);

            buffer.WriteByte(PlayerID);
            buffer.WriteByte(Paused ? 1 : 0);

            return buffer.GetMessageBuffer();
        }

        public override void Unpack(byte[] data)
        {
            Reset(data);
            PlayerID = ReadByte();
            Paused = ReadByte() != 0;
        }
    }
}
