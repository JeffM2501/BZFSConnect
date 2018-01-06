
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BZFlag.Networking.Messages.BZFS.Info;
using BZFlag.Data.Flags;
using BZFlag.Game.Host.Players;
using BZFlag.LinearMath;
using BZFlag.Networking.Messages.BZFS.Flags;

namespace BZFlag.Game.Host.World
{
    public partial class FlagManager : Server.GameState
    {
        public class FlagInstance : FlagUpdateData
        {
            public FlagType Flag = FlagTypeList.None;
            public ServerPlayer Owner = null;

            public bool Grabable()
            {
                lock (this)
                    return Owner == null && (Status == FlagStatuses.FlagInAir || Status == FlagStatuses.FlagOnGround);
            }
        }

        protected Dictionary<int, FlagInstance> ActiveFlags = new Dictionary<int, FlagInstance>();

        protected Dictionary<int, FlagInstance> CarriedFlags = new Dictionary<int, FlagInstance>();
        protected Dictionary<int, FlagInstance> WorldFlags = new Dictionary<int, FlagInstance>();

        protected List<int> EmptyFlagIDs = new List<int>();

        public static readonly int MaxFlagID = 256;

        public FlagInstance[] GetActiveFlags()
        {
            lock (ActiveFlags)
                return ActiveFlags.Values.ToArray();
        }

        public event EventHandler<FlagInstance> FlagAdded = null;
        public event EventHandler<FlagInstance> FlagRemoved = null;

        public event EventHandler<FlagInstance> FlagGrabbed = null;
        public event EventHandler<FlagInstance> FlagDropped = null;

        public delegate void BuildRandomFlagsCallback(FlagManager manager, ServerConfig.FlagInfo flagInfo);
        public BuildRandomFlagsCallback BuildRandomFlags = null;

        public delegate void BuildMapFlagsCallback(FlagManager manager, GameWorld map);
        public BuildMapFlagsCallback BuildMapFlags = null;

        public delegate void SpawnFlagCallback(FlagManager manager, GameWorld map, FlagType flag,  ref Vector3F postion);
        public SpawnFlagCallback ComputeFlagSpawnPoint = SimpleSpawn;

        public FlagManager()
        {
            ComputeFlagDrop = StandardDrop;
            ComputeFlagAdd = StandardDrop;
        }

        protected int GetNewFlagID()
        {
            lock (ActiveFlags)
            {
                if (ActiveFlags.Count > MaxFlagID)
                    return -1;
            }

            lock(EmptyFlagIDs)
            {
                if (EmptyFlagIDs.Count > 0)
                {
                    int id = EmptyFlagIDs[0];
                    EmptyFlagIDs.RemoveAt(0);
                    return id;
                }
            }

            lock (ActiveFlags)
                return ActiveFlags.Count;
        }

        public FlagInstance FindFlagByID(int id)
        {
            lock(ActiveFlags)
            {
                if (ActiveFlags.ContainsKey(id))
                    return ActiveFlags[id];
            }
            return null;
        }

        public void ClearAllFlags()
        {
            lock (ActiveFlags)
            {
                EmptyFlagIDs.Clear();
                ActiveFlags.Clear();
            }
        }

        protected FlagInstance SetupNewFlag(FlagType flag, Vector3F location, bool spawnInAir)
        {
            FlagInstance inst = new FlagInstance();
            inst.Flag = flag;
            inst.Postion = location;
            inst.Owner = null;
            inst.OwnerID = -1;

            if (spawnInAir)
            {
                inst.Status = FlagStatuses.FlagComing;
                inst.LaunchPosition = location;
                inst.LandingPostion = new Vector3F(location.X, location.Y, 0); // TODO, project ray into octree
                inst.FlightEnd = 1;
            }
            else
            {
                inst.Status = FlagStatuses.FlagOnGround;
            }

            inst.FlagID = GetNewFlagID();
            if (inst.FlagID < 0)
                return null;

            lock (ActiveFlags)
            {
                ActiveFlags.Add(inst.FlagID, inst);
                WorldFlags.Add(inst.FlagID, inst);
            }

            Logger.Log3("Setup new flag " + inst.FlagID.ToString() + " of type " + flag.FlagAbbv);

            return inst;
        }

        public bool AddFlag(FlagType flag, Vector3F location, bool spawnInAir)
        {
            FlagInstance inst = SetupNewFlag(flag,location,spawnInAir);
            if (inst != null)
                FlagAdded?.Invoke(this, inst);

            MsgFlagUpdate upd = new MsgFlagUpdate();
            upd.FlagUpdates.Add(inst);
            Players.SendToAll(upd, false);

            Logger.Log2("Added new flag " + inst.FlagID.ToString() + " of type " + flag.FlagAbbv);

            return inst != null;
        }

        public bool AddFlag(FlagType flag)
        {
            return AddFlag(flag, GetFlagSpawn(flag), true);
        }

        public bool InitFlag(FlagType flag, Vector3F location)
        {
            return SetupNewFlag(flag, location, false) != null;
        }

        public bool InitFlag(FlagType flag)
        {
            return InitFlag(flag, GetFlagSpawn(flag));
        }

        public void RemoveFlag(FlagInstance flag)
        {
            DropFlag(flag);

            flag.Status = FlagStatuses.FlagNoExist;
            lock (ActiveFlags)
            {
                ActiveFlags.Remove(flag.FlagID);
                lock (WorldFlags)
                {
                    if (WorldFlags.ContainsKey(flag.FlagID))
                        WorldFlags.Remove(flag.FlagID);
                }

                lock (CarriedFlags)
                {
                    if (WorldFlags.ContainsKey(flag.FlagID))
                        WorldFlags.Remove(flag.FlagID);
                }
            }

            MsgFlagUpdate upd = new MsgFlagUpdate();
            upd.FlagUpdates.Add(flag);
            Players.SendToAll(upd, false);

            lock (EmptyFlagIDs)
                EmptyFlagIDs.Add(flag.FlagID);

            FlagRemoved?.Invoke(this, flag);

            Logger.Log2("Removed flag " + flag.FlagID.ToString() + " of type " + flag.Flag.FlagAbbv);
        }

        public MsgNegotiateFlags GetFlagNegotiation(MsgNegotiateFlags inFlags)
        {
            MsgNegotiateFlags outFlags = new MsgNegotiateFlags();

            foreach (var flag in FlagTypeList.Flags)
            {
                if (flag.FlagAbbv != string.Empty && !inFlags.Contains(flag.FlagAbbv))
                    outFlags.FlagAbrevs.Add(flag.FlagAbbv);
            }

            return outFlags;
        }
        public void SendInitialFlagUpdate(ServerPlayer player)
        {
            int maxFlagsPerUpdate = 10;

            MsgFlagUpdate msg = new MsgFlagUpdate();

            bool sentOne = false;

            foreach (FlagInstance flag in GetActiveFlags())
            {
                if (msg.FlagUpdates.Count == maxFlagsPerUpdate)
                {
                    sentOne = true;
                    player.SendMessage(msg);
                    msg = new MsgFlagUpdate();
                }

                msg.FlagUpdates.Add(flag);
            }

            if (!sentOne || msg.FlagUpdates.Count > 0)
                player.SendMessage(msg);
        }

        public void Update(Data.Time.Clock gameTime)
        {
            foreach (var flag in GetActiveFlags())
            {
                if (flag.Status == FlagStatuses.FlagNoExist)
                    RemoveFlag(flag);

                if (flag.Owner != null)
                {
                    // check shot limts?
                }
                else
                {
                    switch(flag.Status)
                    {
                        case FlagStatuses.FlagGoing:
                                // flag is being despawned
                            break;

                        case FlagStatuses.FlagComing:
                            break;

                        case FlagStatuses.FlagInAir:
                            break;

                        case FlagStatuses.FlagOnGround:
                            break;
                    }
                }

            }
        }

        public void SetupIniitalFlags()
        {
            if (ConfigData.Flags.SpawnRandomFlags)
                BuildRandomFlags?.Invoke(this, ConfigData.Flags);

            if (ConfigData.Flags.SpawnRandomFlags && !ConfigData.Flags.RandomFlags.OverrideMapFlags && false) // repolace with check for map flags object
                BuildMapFlags?.Invoke(this, World);
        }

        public Vector3F GetFlagSpawn(FlagType flag)
        {
            Vector3F pos = new Vector3F(0, 0, 0);

            ComputeFlagSpawnPoint?.Invoke(this, World, flag, ref pos);
            return pos;
        }

        protected static void SimpleSpawn(FlagManager manager, GameWorld map, FlagType flag, ref Vector3F postion)
        {
            float dummy = 0;
            map.GetSpawn(ref postion, ref dummy);
        }
    }
}
