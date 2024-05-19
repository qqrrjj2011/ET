using System;
using System.Collections.Generic;
using ET.EventType;
using Unity.Mathematics;

namespace ET.Client
{
    [Invoke(TimerInvokeType.BattleRound)]
    public class AdventureBattleRoundTimer : ATimer<AdventureComponent>
    {
        protected override void Run(AdventureComponent t)
        {
            t?.PlayOneBattleRound().Coroutine();
        }
    }
    
    [FriendOf(typeof(AdventureComponent))]
    public class AdventureComponentDestroySystem : DestroySystem<AdventureComponent>
    {
        protected override void Destroy(AdventureComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.BattleTimer);
            self.BattleTimer = 0;
            self.Round = 0;
            self.EnemyIdList.Clear();
            self.AliveEnemyIdList.Clear();
            self.Random = null;
        }
    }
    
    [FriendOf(typeof(AdventureComponent))]
    public static class AdventureComponentSystem
    {
        public static void SetBattleRandomSeed(this AdventureComponent self)
        {
            uint seed = (uint) UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene()).GetComponent<NumericComponent>().GetAsInt(NumericType.BattleRandomSeed);
            if (self.Random == null)
            {
                self.Random = new SRandom(seed);
            }
            else
            {
                self.Random.SetRandomSeed(seed);
            }
        }
        
        public static void ResetAdventure(this AdventureComponent self)
        {
            for ( int i = 0; i < self.EnemyIdList.Count; i++ )
            {
                self.Root().CurrentScene().GetComponent<UnitComponent>().Remove(self.EnemyIdList[i]);
            }
            
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.BattleTimer);
            self.BattleTimer = 0;
            self.Round = 0;
            self.EnemyIdList.Clear();
            self.AliveEnemyIdList.Clear();
            
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
            int maxHp = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MaxHp);
            unit.GetComponent<NumericComponent>().Set(NumericType.Hp,maxHp);
            unit.GetComponent<NumericComponent>().Set(NumericType.IsAlive ,1);
            
            EventSystem.Instance.PublishAsync(self.Root().CurrentScene(),new ET.EventType.AdventureRoundReset() {ZoneScene = self.Root()}).Coroutine();
        }
        
        public static async ETTask StartAdventure(this AdventureComponent self)
        {
            self.ResetAdventure();
            await self.CreateAdventureEnemy();
            self.ShowAdventureHpBarInfo(true);
            self.BattleTimer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNow() + 500, TimerInvokeType.BattleRound, self);
        }
        
        
        public static  void  ShowAdventureHpBarInfo(this AdventureComponent self,bool isShow)
        {
            Unit myUnit = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
            EventType.ShowAdventureHpBar.Instance.Unit = myUnit;
            ShowAdventureHpBar.Instance.isShow = isShow;
            EventSystem.Instance.Publish(self.Root(),ShowAdventureHpBar.Instance);
            for ( int i = 0; i < self.EnemyIdList.Count; i++ )
            {
                Unit monsterUnit =  self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.EnemyIdList[i]);
                ShowAdventureHpBar.Instance.Unit = monsterUnit;
                EventSystem.Instance.Publish(self.Root(),ShowAdventureHpBar.Instance);
                 
            }
        }
        
        
        public static async ETTask CreateAdventureEnemy(this AdventureComponent self)
        {
            //根据关卡ID创建出怪物
            Unit unit   = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
            int levelId = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Level);

            BattleLevelConfig battleLevelConfig = BattleLevelConfigCategory.Instance.Get(levelId);
            for ( int i = 0; i < battleLevelConfig.MonsterIds.Length; i++ )
            {
                Unit monsterUnit     = await UnitFactory.CreateMonster(self.Root().CurrentScene(), battleLevelConfig.MonsterIds[i]);
                monsterUnit.Position = new float3(1.5f, -2+i, 0);
                self.EnemyIdList.Add(monsterUnit.Id);
            }
        }
         
        public static async ETTask  PlayOneBattleRound(this AdventureComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
            if ( self.Round % 2 == 0 )
            {
                //玩家回合
                Unit monsterUnit = self.GetTargetMonsterUnit();
                EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleRoundView()
                {
                    scene = self.Root(), AttackUnit = unit, TargetUnit = monsterUnit
                }).Coroutine();
                
                await EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleRound()
                {
                    scene = self.Root(), AttackUnit = unit, TargetUnit = monsterUnit
                });
                
                await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            }
            else
            {
                //敌人回合
                for ( int i = 0; i < self.EnemyIdList.Count; i++ )
                {
                    if (!unit.IsAlive())
                    {
                        break;
                    }
                    
                    Unit monsterUnit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.EnemyIdList[i]);

                    if (!monsterUnit.IsAlive())
                    {
                        continue;
                    }
                    
                    EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleRoundView()
                    {
                        scene = self.Root(), AttackUnit = monsterUnit, TargetUnit = unit
                    }).Coroutine();
                    
                    await EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleRound()
                    {
                        scene = self.Root(), AttackUnit = monsterUnit, TargetUnit = unit
                    });
                    
                    await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
                }
            }
            
            self.BattleRoundOver();
        }
        
        
        public static  void  BattleRoundOver(this AdventureComponent self)
        {
            ++self.Round;
            BattleRoundResult battleRoundResult = self.GetBattleRoundResult();
            Log.Debug("当前回合结果:" + battleRoundResult);
            switch ( battleRoundResult )
            {
                case BattleRoundResult.KeepBattle:
                {
                    self.BattleTimer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNow() + 500, TimerInvokeType.BattleRound, self);
                }
                    break;
                case BattleRoundResult.WinBattle:
                {
                    Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
                    EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleOver() { scene = self.Root(), WinUnit = unit }).Coroutine();
                }
                    break;
                case BattleRoundResult.LoseBattle:
                {
                    for (int i = 0; i < self.EnemyIdList.Count; i++)
                    {
                        Unit monsterUnit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.EnemyIdList[i]);
                        if (!monsterUnit.IsAlive())
                        {
                            continue;
                        }
                        EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleOver() { scene = self.Root(), WinUnit = monsterUnit }).Coroutine();
                    }
                }
                    break;
            }
            
            EventSystem.Instance.PublishAsync(self.Root(),new ET.EventType.AdventureBattleReport()
            {
                Round = self.Round, BattleRoundResult = battleRoundResult, scene = self.Root()
            }).Coroutine();
        }
        
        public static Unit GetTargetMonsterUnit(this AdventureComponent self)
        {
           self.AliveEnemyIdList.Clear();
           for ( int i = 0; i < self.EnemyIdList.Count; i++ )
           {
               Unit monsterUnit = self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.EnemyIdList[i]);
               if ( monsterUnit.IsAlive() )
               {
                   self.AliveEnemyIdList.Add(monsterUnit.Id);
               }
           }

           if ( self.AliveEnemyIdList.Count <= 0 )
           {
               return null;
           }
           return self.Root().CurrentScene().GetComponent<UnitComponent>().Get(self.AliveEnemyIdList[0]);
        }
        
        public static BattleRoundResult GetBattleRoundResult(this AdventureComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene());
            if ( !unit.IsAlive() )
            {
                return BattleRoundResult.LoseBattle;
            }
            
            Unit monsterUnit = self.GetTargetMonsterUnit();
            if ( monsterUnit == null )
            {
                return BattleRoundResult.WinBattle;
            }
            
            return BattleRoundResult.KeepBattle;
        }
    }
}