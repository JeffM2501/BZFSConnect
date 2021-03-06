﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZFlag.Networking.Messages.BZFS.Info
{
    public class MsgAdminInfo : NetworkMessage
    {
        public class IPRecord : EventArgs
        {
            public int PlayerID = 0;
            public byte[] IPAddress = new byte[0];
        }

        public List<IPRecord> Records = new List<IPRecord>();


        public MsgAdminInfo()
        {
            Code = CodeFromChars("ai");
        }

        public override byte[] Pack()
        {
            throw new NotImplementedException();
        }

        public override void Unpack(byte[] data)
        {
            Reset(data);

            int count = ReadByte();

            for (int i = 0; i < count; i++)
            {
                IPRecord rec = new IPRecord();

                int size = ReadByte();
                rec.PlayerID = ReadByte();
                rec.IPAddress = ReadBytes(size);

                Records.Add(rec);
            }
        }
    }
}
