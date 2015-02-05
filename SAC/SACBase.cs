#region
// Copyright 2014 - 2015 LeagueSharp
// SACBase.cs is part of SAC.
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
    internal abstract class SACBase
    {
        protected SACBase()
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
            Game.OnGameUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Interrupter2.OnInterruptableTarget += OnPossibleToInterrupt;
            CreateMenu();
        }

        internal virtual void OnLoad(EventArgs args) { }
        internal virtual void OnUpdate(EventArgs args) { }
        internal virtual void OnDraw(EventArgs args) { }
        internal virtual void OnPossibleToInterrupt(Obj_AI_Hero hero, Interrupter2.InterruptableTargetEventArgs args) { }

        public static Obj_AI_Hero MyHero = G.User;

        public static Menu Menu = G.Menu;

        public static Menu ComboMenu = G.ComboMenu;
        public static Menu LaningMenu = G.LaningMenu;

        public Spell Q = G.Q;
        public Spell W = G.W;
        public Spell E = G.E;
        public Spell R = G.R;

        public List<Spell> SpellList = G.SpellList; 

        public static Obj_AI_Base GetTarget(float range, TargetSelector.DamageType damageType)
        {
            return TargetSelector.GetTarget(range, damageType);
        }

        public static TargetSelector Ts;

        public Orbwalking.Orbwalker Orbwalker { get; set; }

        public string MenuName = "sac.menu." + G.ChampionName;

        internal void CreateMenu()
        {
            Menu = new Menu("SAC: " + G.ChampionName, MenuName, true);
            Menu.AddSubMenu(new Menu("Orbwalking", "orbwalkingmenu"));

            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("orbwalkingmenu"));

            TargetSelector.AddToMenu(Menu.AddSubMenu(new Menu("Target Selector", "tsmenu")));

            ComboMenu = Menu.AddSubMenu(new Menu("Combo", "combomenu"));
            LaningMenu = Menu.AddSubMenu(new Menu("Laning", "laningmenu"));
            Menu.AddToMainMenu();
        }
    }
}
