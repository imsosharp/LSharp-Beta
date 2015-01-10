﻿/* Autoplay Plugin of h3h3's AIO Support
*
* All credits go to him. I only wrote this and
* MetaHandler.cs.
* The core is always updated to latest version.
* which you can find here:
* https://github.com/h3h3/LeagueSharp/tree/master/Support
*/

using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using Support.Util;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Support
{
    internal class Autoplay
    {
        private const int Blue = 200;
        private const int Purple = -200;
        public static Obj_AI_Hero Bot = ObjectManager.Player;
        public static Obj_AI_Hero Carry;
        public static Obj_AI_Hero NearestAllyHero;
        public static Obj_AI_Turret NearestAllyTurret;
        public static Obj_AI_Hero Jungler;
        public static readonly Random Rand = new Random((42 / 13 * DateTime.Now.Millisecond) + DateTime.Now.Second + Environment.TickCount);
        private static Obj_AI_Hero _tempcarry;
        private static Vector2 _lanepos;
        private static int _chosen;
        private static int _safe;
        private static Vector2 _frontline;
        private static Vector2 _safepos;
        private static Vector2 _saferecall;
        private static Vector2 _orbwalkingpos;
        private static int _loaded;
        private static bool _byPassLoadedCheck = false;
        private static int _randSeconds, _randRange, _stepTime;
        private const float LOWHEALTHRATIO = 0.3f;
        private const float LOWMANARATIO = 0.1f;
        private const float LOWHEALTHRATIOIFLOWMANARATIO = 0.6f;

        public Autoplay()
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
            Game.OnGameUpdate += OnUpdate;
            Game.OnGameEnd += OnGameEnd;
        }

        private static void OnGameLoad(EventArgs args)
        {
            _loaded = Environment.TickCount;
            _stepTime = Environment.TickCount;
            var map = Utility.Map.GetMap();
            if (map != null && map.Type == Utility.Map.MapType.SummonersRift)
            {
                if (Bot.Team == GameObjectTeam.Order)
                {
                    _chosen = Blue + Rand.Next(-76, 76);
                    _safe = Purple + Rand.Next(-67, 67);
                    _lanepos.X = 11376 + Rand.Next(-50, 50);
                    _lanepos.Y = 1062 + Rand.Next(-50, 50);
                }
                if (Bot.Team == GameObjectTeam.Chaos)
                {
                    _chosen = Purple + Rand.Next(-67, 67);
                    _safe = Blue + Rand.Next(-76, 76);
                    _lanepos.X = 13496 + Rand.Next(-50, 50);
                    _lanepos.Y = 4218 + Rand.Next(-50, 50);
                }
            }
            else
            {
                _byPassLoadedCheck = true;
                if (Bot.Team == GameObjectTeam.Order)
                {
                    _chosen = Blue;
                    _safe = Purple;
                }
                if (Bot.Team == GameObjectTeam.Chaos)
                {
                    _chosen = Purple;
                    _safe = Blue;
                }
            }
            AutoLevel levelUpSpells = new AutoLevel(TreesAutoLevel.GetSequence());
            AutoLevel.Enabled(true);
            FileHandler.DoChecks();
            MetaHandler.LoadObjects();
        }

        private static void OnUpdate(EventArgs args)
        {
            DoAutoplay();
            MetaHandler.DoChecks();
            MetaHandler.UpdateObjects();
        }

        public static void OnGameEnd(EventArgs args)
        {
            //Game.Say("gg"); nope
        }

        private static bool IsBotSafe()
        {
            var map = Utility.Map.GetMap();
            if (map != null && map.Type == Utility.Map.MapType.HowlingAbyss)
            {
                return true;
            }
            if (Bot.InFountain())
            {
                return (Bot.Health > Bot.MaxHealth * 0.9f);
            }
            if (Bot.Mana < Bot.MaxMana * LOWMANARATIO)
            {
                return Bot.Health > Bot.MaxHealth * LOWHEALTHRATIOIFLOWMANARATIO && !Bot.IsRecalling();
                    //&& !(Bot.Gold > (2200 + Rand.Next(100, 1100)));
            }
            return (Bot.Health > Bot.MaxHealth * LOWHEALTHRATIO) && !Bot.IsRecalling();
                //&& !(Bot.Gold > (2200 + Rand.Next(100, 1100)));

        }

        public static void DoAutoplay()
        {
            var timeElapsed = Environment.TickCount - _loaded;
            if (!Bot.IsDead)
            {
                try
                {
                    var turret = MetaHandler.EnemyTurrets.FirstOrDefault(t => t.Distance(Bot) < 1200);
                    if (Bot.UnderTurret(true) && MetaHandler.NearbyAllyMinions(turret, 750) > 2)
                    {
                            if (turret.Distance(Bot) < Bot.AttackRange)
                                Bot.IssueOrder(GameObjectOrder.AttackUnit, turret);
                    }
                    else
                    {
                        Obj_AI_Hero target = TargetSelector.GetTarget(
                            Bot.AttackRange, TargetSelector.DamageType.Physical);
                        if (target != null && target.IsValid && target.IsDead)
                        {
                            Bot.IssueOrder(GameObjectOrder.AttackUnit, target);
                        }
                    }
                    if (Bot.UnderTurret(true) && MetaHandler.NearbyAllyMinions(turret, 750) < 2)
                    {
                        _safepos.X = (Bot.Position.X + _safe);
                        _safepos.Y = (Bot.Position.Y + _safe);
                        Bot.IssueOrder(GameObjectOrder.MoveTo, _safepos.To3D());
                    }
                    #region Carry is null
                    if (Carry == null && timeElapsed > 15000 && timeElapsed < 135000 && !_byPassLoadedCheck)
                    {
                        if (Bot.InFountain() || Bot.Distance(_lanepos) > 400)
                        {
                            Bot.IssueOrder(GameObjectOrder.MoveTo, _lanepos.To3D());
                        }
                        if (Bot.Distance(_lanepos) < 1000)
                        {
                            WalkAround(_lanepos.To3D());
                            if (MetaHandler.AllyHeroes.FirstOrDefault(hero => !hero.IsMe && hero.Distance(Bot) < 8000 && !MetaHandler.HasSmite(hero)) != null)
                            {
                                Carry = MetaHandler.AllyHeroes.FirstOrDefault(hero => !hero.IsMe && hero.Distance(Bot) < 8000 && !MetaHandler.HasSmite(hero));
                            }
                        }
                    }
                    if (_byPassLoadedCheck && Carry == null)
                    {
                        if (MetaHandler.AllyHeroes.FirstOrDefault(hero => !hero.IsMe && !MetaHandler.HasSmite(hero)) != null)
                        {
                            Carry = MetaHandler.AllyHeroes.FirstOrDefault(hero => !hero.IsMe && !MetaHandler.HasSmite(hero));
                        }
                    }
                    #endregion
                    #region Carry is dead
                    if (Carry != null)
                    {
                        if (IsBotSafe() && Carry.IsDead || Carry.InFountain())
                        {
                            if (_tempcarry == null || _tempcarry.IsDead || _tempcarry.InFountain())
                            {
                                if (
                                    MetaHandler.AllyHeroes.FirstOrDefault(
                                        hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead && !MetaHandler.HasSmite(hero)) !=
                                    null)
                                {
                                    _tempcarry =
                                        MetaHandler.AllyHeroes.FirstOrDefault(
                                            hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead && !MetaHandler.HasSmite(hero));
                                }
                                if (
                                    MetaHandler.AllyHeroes.FirstOrDefault(
                                        hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead && !MetaHandler.HasSmite(hero)) ==
                                    null &&
                                    MetaHandler.AllyHeroes.FirstOrDefault(
                                        hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead) != null)
                                {
                                    //well fuck, let's follow the jungler -sighs-
                                    _tempcarry =
                                        MetaHandler.AllyHeroes.FirstOrDefault(
                                            hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead);
                                }
                            }
                            if (_tempcarry != null)
                            {

                                Console.WriteLine("Carry dead or afk, following: " + _tempcarry.ChampionName);
                                _frontline.X = _tempcarry.Position.X + _chosen;
                                _frontline.Y = _tempcarry.Position.Y + _chosen;
                                if (!(_tempcarry.UnderTurret(true) && MetaHandler.NearbyAllyMinions(_tempcarry, 400) < 2) && IsBotSafe())
                                {
                                    if (_tempcarry.Distance(Bot) > 450)
                                    {
                                        Bot.IssueOrder(GameObjectOrder.MoveTo, _frontline.To3D());
                                        WalkAround(_tempcarry);
                                    }
                                }
                            }
                        }
                    }
                    #endregion Carry is dead
                    #region Following
                    if (Carry != null && !Carry.IsDead && !Carry.InFountain() && IsBotSafe() && !(Carry.UnderTurret(true) && MetaHandler.NearbyAllyMinions(Carry, 400) < 2))
                    {
                        Console.WriteLine("All good, following: " + Carry.ChampionName);
                        _frontline.X = Carry.Position.X + _chosen;
                        _frontline.Y = Carry.Position.Y + _chosen;
                        if (Carry.Distance(Bot) > 450)
                        {
                            Bot.IssueOrder(GameObjectOrder.MoveTo, _frontline.To3D());
                        }

                        WalkAround(Carry);
                    }
                    #endregion Following
                    #region Carry not found
                    if (timeElapsed > 135000 &&
                        Carry == null && IsBotSafe())
                    {
                        if (_tempcarry == null || _tempcarry.IsDead || _tempcarry.InFountain())
                        {
                            if (
                                MetaHandler.AllyHeroes.FirstOrDefault(
                                    hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead && !MetaHandler.HasSmite(hero)) != null)
                            {
                                _tempcarry =
                                    MetaHandler.AllyHeroes.FirstOrDefault(
                                        hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead && !MetaHandler.HasSmite(hero));
                            }
                            if (
                                MetaHandler.AllyHeroes.FirstOrDefault(
                                    hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead && !MetaHandler.HasSmite(hero)) == null &&
                                MetaHandler.AllyHeroes.FirstOrDefault(
                                    hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead) != null)
                            {
                                //well fuck, let's follow the jungler -sighs-
                                _tempcarry =
                                    MetaHandler.AllyHeroes.FirstOrDefault(
                                        hero => !hero.IsMe && !hero.InFountain() && !hero.IsDead);
                            }
                        }
                        if (_tempcarry != null)
                        {
                            Console.WriteLine("Carry not found, following: " + _tempcarry.ChampionName);
                            _frontline.X = _tempcarry.Position.X + _chosen;
                            _frontline.Y = _tempcarry.Position.Y + _chosen;
                            if (!(_tempcarry.UnderTurret(true) && MetaHandler.NearbyAllyMinions(_tempcarry, 400) < 2) && IsBotSafe())
                            {
                                if (Bot.Distance(_frontline) > 450)
                                {
                                    Bot.IssueOrder(GameObjectOrder.MoveTo, _frontline.To3D());
                                }
                                WalkAround(_tempcarry);
                            }
                        }
                    }
                    #endregion
                    #region Lowhealth mode
                    if (!IsBotSafe() && !Bot.InFountain())
                    {
                        if (NearestAllyTurret == null)
                        {
                            NearestAllyTurret = MetaHandler.AllyTurrets.FirstOrDefault();
                        }
                        if (NearestAllyTurret != null)
                        {
                            _saferecall.X = NearestAllyTurret.Position.X + _safe;
                            _saferecall.Y = NearestAllyTurret.Position.Y;
                            if (Bot.Position.Distance(_saferecall.To3D()) < 200)
                            {
                                Bot.Spellbook.CastSpell(SpellSlot.Recall);
                            }
                            else
                            {

                                Bot.IssueOrder(GameObjectOrder.MoveTo, _saferecall.To3D());
                            }

                        }

                    }
                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        } //end of DoAutoplay()

        private static void WalkAround(Vector3 pos)
        {
            _randRange = Rand.Next(-267, 276);
            _randSeconds = Rand.Next(1000, 4000);
            if (Environment.TickCount - _stepTime >= _randSeconds)
            {
                if (Bot.Team == GameObjectTeam.Order)
                {
                    int orbwalkingAdditionInteger = _randRange * (-1);
                    _orbwalkingpos.X = Bot.Position.X + orbwalkingAdditionInteger;
                    _orbwalkingpos.Y = Bot.Position.Y + orbwalkingAdditionInteger;
                }
                else
                {
                    int orbwalkingAdditionInteger = _randRange;
                    _orbwalkingpos.X = Bot.Position.X + orbwalkingAdditionInteger;
                    _orbwalkingpos.Y = Bot.Position.Y + orbwalkingAdditionInteger;
                }
                if (_orbwalkingpos != null)
                {
                    Bot.IssueOrder(GameObjectOrder.MoveTo, _orbwalkingpos.To3D());
                    _stepTime = Environment.TickCount;
                }
            }

        } //end of WalkAround()

        private static void WalkAround(Obj_AI_Hero follow)
        {
            _randRange = Rand.Next(-267, 276);
            _randSeconds = Rand.Next(500, 3500);
            if (Environment.TickCount - _stepTime >= _randSeconds)
            {
                if (Bot.Team == GameObjectTeam.Order)
                {
                    int orbwalkingAdditionInteger = _randRange * (-1);
                    _orbwalkingpos.X = follow.Position.X + orbwalkingAdditionInteger;
                    _orbwalkingpos.Y = follow.Position.Y + orbwalkingAdditionInteger;
                }
                else
                {
                    int orbwalkingAdditionInteger = _randRange;
                    _orbwalkingpos.X = follow.Position.X + orbwalkingAdditionInteger;
                    _orbwalkingpos.Y = follow.Position.Y + orbwalkingAdditionInteger;
                }
                if (_orbwalkingpos != null)
                {
                    Bot.IssueOrder(GameObjectOrder.MoveTo, _orbwalkingpos.To3D());
                    _stepTime = Environment.TickCount;
                }
            }

        } //end of WalkAround(Obj_AI_Hero)
    }
}