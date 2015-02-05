#region
// Copyright 2014 - 2015 LeagueSharp
// SACMathWiz.cs is part of SAC.
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

namespace SAC.SACUtils
{
    internal static class SACMathWiz
    {
        internal static float GetSpellDmg(this Spell spell, Obj_AI_Base target)
        {
            return spell.GetDamage(target);
        }

        internal static bool WillKill(this Spell spell, Obj_AI_Base target)
        {
            if (spell.GetSpellDmg(target) > target.Health)
            {
                return true;
            }
            return false;
        }

        internal static float GetComboDmg(this Obj_AI_Hero hero, Obj_AI_Base target)
        {
            var result = new float();
            foreach (var spell in hero.Spellbook.Spells)
            {
                if (spell.IsReady())
                {
                    result += (float)hero.GetSpellDamage(target, spell.Name);
                }
            }
            return result;
        }

        internal static SpellData GetSpellData(this Obj_AI_Hero hero, SpellSlot spellslot)
        {
            return SpellData.GetSpellData(hero.GetSpell(spellslot).Name);
        }

        internal static bool UltraManaSavingModeEnabled()
        {
            if (G.User.Mana < G.ComboMana)
            {
                return true;
            }
            return false;
        }
    }
}
