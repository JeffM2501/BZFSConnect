
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;

using BZFlag.Networking.Messages;
using BZFlag.Networking.Common;
using BZFlag.Networking.Messages.BZFS.Info;
using BZFlag.Data.Teams;

namespace BZFlag.Game.Host.Players
{
    public class ServerPlayer : Peer
    {
        public int PlayerID = 0;
        public string BZID = string.Empty;

        public object Tag = 0;

        public bool Accepted = false;
        public string RejectionReason = string.Empty;

        public string Callsign = string.Empty;
        public string Token = string.Empty;
        public string Motto = string.Empty;

        public TeamColors DesiredTeam = TeamColors.AutomaticTeam;
        public TeamColors ActualTeam = TeamColors.AutomaticTeam;

        public class ScoreInfo
        {
            public int Wins = 0;
            public int Losses = 0;
            public int TeamKills = 0;
        }
        public ScoreInfo Score = new ScoreInfo();

        public bool NeedStartupInfo = true;

        public MsgNegotiateFlags ClientFlagList = null;

        public PlayerManager.PlayerInfo Info = null; 

        public enum AuthStatuses
        {
            Unknown,
            NoneProvided,
            InProgress,
            Valid,
            Failed,
        }

        public AuthStatuses AuthStatus = AuthStatuses.Unknown;
        public List<string> GroupMemberships = new List<string>();

        protected NetworkStream NetStream = null;

        public TCPConnectionManager.PendingClient ConnectionData = null;

        public enum UDPConenctionStatuses
        {
            Unknown,
            RequestSent,
            Connected,
        }
        public UDPConenctionStatuses UDPStatus = UDPConenctionStatuses.Unknown;

        public bool SentSettings = false;

        public ServerPlayer(TCPConnectionManager.PendingClient pc)
        {
            ConnectionData = pc;
            Link(ConnectionData.ClientConnection);
        }

        public void ProcessUDPMessage(NetworkMessage msg)
        {
            msg.FromUDP = true;
            InboundMessageProcessor.CompleteMessage(msg);
        }
    }
}
