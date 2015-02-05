#region
// Copyright 2014 - 2015 LeagueSharp
// SACGlobals.cs is part of SAC.
// 
// SAC is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SACGlobals.cs is distributed in the hope that it will be useful,
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
    internal static class G
    {
        internal static Obj_AI_Hero User { get { return ObjectManager.Player; } }
        internal static string ChampionName { get { return User.BaseSkinName; } }
        internal static Spellbook SpellBook { get { return User.Spellbook; } }
        internal static string AllyTeam { get { return User.Team.ToString(); } }
        internal static string EnemyTeam { get { return User.Team == GameObjectTeam.Order ? GameObjectTeam.Chaos.ToString() : GameObjectTeam.Order.ToString(); } }
        internal static SpellSlot QSpellSlot { get { return SpellSlot.Q; } }
        internal static SpellSlot WSpellSlot { get { return SpellSlot.Q; } }
        internal static SpellSlot ESpellSlot { get { return SpellSlot.Q; } }
        internal static SpellSlot RSpellSlot { get { return SpellSlot.Q; } }
        internal static Spell QSpell { get { return new Spell(QSpellSlot); }}
        internal static Spell WSpell { get { return new Spell(WSpellSlot); } }
        internal static Spell ESpell { get { return new Spell(ESpellSlot); } }
        internal static Spell RSpell { get { return new Spell(RSpellSlot); } }
        internal static float QMana { get { return QSpell.Instance.ManaCost; } }
        internal static float WMana { get { return WSpell.Instance.ManaCost; } }
        internal static float EMana { get { return ESpell.Instance.ManaCost; } }
        internal static float RMana { get { return RSpell.Instance.ManaCost; } }
        internal static float ComboMana { get { return QMana + WMana + EMana; } }
        internal static float QDmg(Obj_AI_Base target) { return QSpell.GetDamage(target); }
        internal static float WDmg(Obj_AI_Base target) { return WSpell.GetDamage(target); }
        internal static float EDmg(Obj_AI_Base target) { return ESpell.GetDamage(target); }
        internal static float RDmg(Obj_AI_Base target) { return RSpell.GetDamage(target); }
        internal static float TotalDmg(Obj_AI_Base target) { return SACMathWiz.TotalDmg(target); }
    }
}
