﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LeagueSharp;
using LeagueSharp.Common;

namespace Support
{
    internal class FileHandler
    {
        private static string _cBuildsPath = Config.LeagueSharpDirectory + @"\AutoSharpporting\";
        private static string _theFile;

        public static void DoChecks()
        {
            _theFile = _cBuildsPath + Utility.Map.GetMap().Type + @"\" + Autoplay.Bot.BaseSkinName + ".txt";
            Game.PrintChat(_theFile);
            if (!Directory.Exists(_cBuildsPath))
            {
                Directory.CreateDirectory(_cBuildsPath);
                Directory.CreateDirectory(_cBuildsPath + Utility.Map.MapType.CrystalScar);
                Directory.CreateDirectory(_cBuildsPath + Utility.Map.MapType.HowlingAbyss);
                Directory.CreateDirectory(_cBuildsPath + Utility.Map.MapType.SummonersRift);
                Directory.CreateDirectory(_cBuildsPath + Utility.Map.MapType.TwistedTreeline);
                Directory.CreateDirectory(_cBuildsPath + Utility.Map.MapType.Unknown);
            }
            if (ExistsCustomBuild())
            {
                Game.PrintChat("Found custom build");
                var contents = File.ReadAllText(_theFile);
                string[] separator = { "," };
                string[] itemsStringArray = contents.Split(separator, StringSplitOptions.None);
                int[] items = new int[itemsStringArray.Count()];
                for(var i = 0; i < itemsStringArray.Count(); i++)
                {
                    //Int32.TryParse(_itemsStringArray[i], out Items[i]);
                    items[i] = Convert.ToInt32(itemsStringArray[i]);
                }
                MetaHandler.CustomBuild = GetCustomBuild(items);
                foreach (var i in GetCustomBuild(items))
                {
                    Game.PrintChat(i.ToString());
                }
                foreach (var i in items)
                {
                    Game.PrintChat(i.ToString());
                }
                foreach (var i in itemsStringArray)
                {
                    Game.PrintChat(i);
                }
            }
        }

        public static bool ExistsCustomBuild()
        {
            return File.Exists(_theFile);
        }

        public static ItemId[] GetCustomBuild(int[] itemsArray)
        {
            ItemId[] localCopy = { };
                for (var i = 0; i < itemsArray.Count(); i++)
                {
                    localCopy[i] = (ItemId) itemsArray[i];
                }
                return localCopy;
        }
    }
}
