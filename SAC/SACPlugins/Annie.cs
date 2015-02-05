#region
// Copyright 2014 - 2015 LeagueSharp
// SACPlugins\Annie is part of SAC.
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

namespace SAC.SACPlugins
{
    internal class Annie : SACBase
    {
        public Annie()
        {
            Q = new Spell(SpellSlot.Q, 650);
            W = new Spell(SpellSlot.W, 625);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R, 600);

            Q.SetTargetted(250, 1400);
            W.SetSkillshot(600, (float)(50 * Math.PI / 180), float.MaxValue, false, SkillshotType.SkillshotCone);
            R.SetSkillshot(250, 200, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        internal override void OnUpdate(EventArgs args)
        {
            var QTarget = Target(Q.Range, TargetSelector.DamageType.Magical);
            var WTarget = Target(W.Range, TargetSelector.DamageType.Magical);
            var RTarget = Target(R.Range, TargetSelector.DamageType.Magical);

            if (Q.ShouldUse(QTarget).Tick() == BehaviorState.Success)
            {
                Q.CastSpell(QTarget).Tick();
            }
            if (W.ShouldUse(WTarget).Tick() == BehaviorState.Success)
            {
                W.CastSpell(WTarget).Tick();
            }
            if (R.ShouldUse(RTarget).Tick() == BehaviorState.Success)
            {
                R.CastSpell(RTarget).Tick();
            }

        }
    }
}
