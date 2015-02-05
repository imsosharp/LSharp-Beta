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
using System.IO;
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
        internal static void Print(string message, params object[] @params)
        {
            Game.PrintChat("<font color='#D859CD'>SAC: </font>" + "<font color='#adec00'>" + message + "</font>");
        }

        internal static void PrintWarning(string message, params object[] @params)
        {
            Game.PrintChat("<font color='#FF0000'>SAC: </font>" + "<font color='#adec00'>" + message + "</font>");
        }

        internal static void SACUpdater()
        {
            Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var installedVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        var request = WebRequest.Create("https://raw.githubusercontent.com/imsosharp/LeagueSharp/master/SAC/Properties/AssemblyInfo.cs");
                        var response = request.GetResponse();
                        if (response.GetResponseStream() == null) { PrintWarning("Network unreacheable"); return; }

                        var streamReader = new StreamReader(response.GetResponseStream());
                        var versionPattern = @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]";
                        Match match;
                        using (streamReader)
                        {
                            match = new Regex(versionPattern).Match(streamReader.ReadToEnd());
                            Version latestVersion;
                            if (match.Success)
                            {
                                latestVersion =
                                    new Version(
                                        string.Format(
                                            "{0}.{1}.{2}.{3}", match.Groups[1], match.Groups[2], match.Groups[3],
                                            match.Groups[4]));
                                if (installedVersion != latestVersion)
                                {
                                    PrintWarning("A new SAC version has been released. Please update to v.{0}!</font>", latestVersion);
                                    PrintWarning("Outdated SAC version loaded!");
                                }
                                else
                                {

                                    Print(@"You have the latest version. GLHF ^^");
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