﻿using BZFlag.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using BZFlag.IO.BZW;

namespace BZFlag.Game.Host.World
{
    public class GameWorld
    {
        public WorldMap Map = new WorldMap();

        public string MapHash = string.Empty;
        public byte[] WorldData = null;

        public void LoadBZWFile(string mapFile)
        {
            FileInfo file = new FileInfo(mapFile);
            var fs = file.OpenText();

            BZFlag.IO.BZW.Reader.ReadMap(fs);
            fs.Close();


        }
    }
}