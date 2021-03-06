using BZFlag.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BZFlag.IO.BZW;
using BZFlag.LinearMath;

namespace BZFlag.Game.Host.World
{
    public class GameWorld
    {
        public WorldMap Map = new WorldMap();

        public bool IsRandom = true;

        public string MapHash = string.Empty;
        protected byte[] WorldData = null;

        public Random RNG { get; protected set; } = new Random();

        public void LoadBZWFile(string mapFile)
        {
            FileInfo file = new FileInfo(mapFile);
            var fs = file.OpenText();

            BZFlag.IO.BZW.Reader.ReadMap(fs);
            fs.Close();
        }

        public byte[] GetWorldData()
        {
            if (WorldData == null)
                WorldData = new BZFlag.IO.BZW.Binary.WorldPacker(Map).Pack();

            return WorldData;
        }

        protected float GetRandomPostion(float size)
        {
            return (float)(RNG.NextDouble() * (size * 2)) - size;
        }

        public bool GetSpawn(ref Vector3F position, ref float rotation)
        {
            position = new Vector3F(GetRandomPostion(Map.WorldInfo.Size * 0.6f), GetRandomPostion(Map.WorldInfo.Size * 0.6f), 0);
            rotation = (float)(RNG.NextDouble() * Math.PI * 2);
            return true;
        }
    }
}
