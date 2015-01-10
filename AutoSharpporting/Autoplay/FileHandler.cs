using System;
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
        public static ItemId[] CustomShopList;

        public static void DoChecks()
        {
            _theFile = _cBuildsPath + Utility.Map.GetMap().Type + @"\" + Autoplay.Bot.BaseSkinName + ".txt";
            Game.PrintChat("Loaded: " + _theFile);
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
                /*Game.PrintChat("Found custom build");
                var contents = File.ReadAllText(_theFile);
                string[] separator = { "," };
                string[] itemsStringArray = contents.Split(separator, StringSplitOptions.None);
                int[] items = new int[itemsStringArray.Count()];*/
                string[] itemsStringArray = File.ReadAllLines(_theFile);
                int[] itemsIntArray = new int[itemsStringArray.Count()];
                CustomShopList = new ItemId[itemsStringArray.Count()];
                foreach (var i in itemsStringArray)
                {
                    Game.PrintChat(i);
                }
                for(var i = 0; i < itemsStringArray.Count(); i++)
                {
                    //Int32.TryParse(_itemsStringArray[i], out Items[i]);
                    itemsIntArray[i] = Convert.ToInt32(itemsStringArray[i]);
                }
                foreach (var i in itemsIntArray)
                {
                    Game.PrintChat(i.ToString());
                }
                for (var i = 0; i < itemsIntArray.Count(); i++)
                {
                    CustomShopList[i] = (ItemId)itemsIntArray[i];
                }
            }
        }

        public static bool ExistsCustomBuild()
        {
            return File.Exists(_theFile);
        }
    }
}
