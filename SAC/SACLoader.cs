#region
// Copyright 2014 - 2015 LeagueSharp
// SACLoader.cs is part of SAC.
// 
// SAC is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SAC is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SAC. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAC;
using SAC.SACUtils;
using SAC.SACBehavior;
using LeagueSharp;
using LeagueSharp.Common;
using BehaviorSharp;
#endregion

namespace SAC
{
    internal class SACLoader
    {
        internal SACLoader()
        {
            CustomEvents.Game.OnGameLoad += load =>
            {
                var championName = ObjectManager.Player.BaseSkinName;
                var plugin = Type.GetType("SAC.Plugins." + championName);

                Game.PrintChat("<font color='#A800AD'>Welcome to SAC</font>");
                SACUtils.SACHelpers.SACUpdater();

                try
                {
                    if (plugin == null)
                    {
                        Game.PrintChat(championName + " not supported yet.");
                        return;
                    }

                    Activator.CreateInstance(plugin);
                    Game.PrintChat("SAC {0} has been succesfully loaded!", championName);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            };

        }

    }
}
