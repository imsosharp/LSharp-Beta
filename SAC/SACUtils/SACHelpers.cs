#region
// Copyright 2014 - 2015 LeagueSharp
// SACHelpers.cs is part of SAC.
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
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAC;
using SAC.SACUtils;
using SAC.SACBehavior;
using LeagueSharp;
using LeagueSharp.Common;
using BehaviorSharp;
using Version = System.Version;
#endregion

namespace SAC.SACUtils
{
    internal class SACHelpers
    {
        internal static void SACUpdater()
        {
            Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        using (var c = new WebClient())
                        {
                            var rawVersion =
                                c.DownloadString(
                                    "https://raw.githubusercontent.com/imsosharp/LeagueSharp/master/SAC/Properties/AssemblyInfo.cs");
                            var match =
                                new Regex(
                                    @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                                    .Match(rawVersion);
                            if (match.Success)
                            {
                                var gitVersion =
                                    new Version(
                                        string.Format(
                                            "{0}.{1}.{2}.{3}", match.Groups[1], match.Groups[2], match.Groups[3],
                                            match.Groups[4]));
                                if (gitVersion != Assembly.GetExecutingAssembly().GetName().Version)
                                {
                                    Game.PrintChat(
                                        "<font color='#15C3AC'>SAC:</font> <font color='#FF0000'>" +
                                        "A new SAC version has been released. Please update to v.{0}!</font>", gitVersion);
                                    Game.PrintChat("<font color='#00CFEB'>SAC:</font> <font color='#FF0000'>outdated SAC version loaded!</font>");
                                }
                                else
                                {
                                    Game.PrintChat("<font color='#15C3AC'>SAC:</font> You have the latest version, GLHF.");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
        }
    }
}