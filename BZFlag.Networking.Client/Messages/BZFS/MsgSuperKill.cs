﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BZFlag.Networking.Messages.BZFS
{
	public class MsgSuperKill : NoPackedDataNetworkMessage
	{
		public MsgSuperKill()
		{
			Code = CodeFromChars("sk");
		}
	}
}
