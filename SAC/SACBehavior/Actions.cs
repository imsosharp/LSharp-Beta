﻿#region
// Copyright 2014 - 2015 LeagueSharp
// SACBehavior\Actions.cs is part of SAC.
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
using BehaviorSharp.Components.Actions;

#endregion

namespace SAC.SACBehavior
{
    internal static class Actions
    {
        internal static BehaviorAction CastSpell(this Spell spell, Obj_AI_Base target)
        {
            return new BehaviorAction(
                () =>
                { 
                    spell.Cast(target);
                    if (!spell.IsReady())
                    {
                        return BehaviorState.Success;
                    }
                    return BehaviorState.Running;
                });
        }
    }
}
