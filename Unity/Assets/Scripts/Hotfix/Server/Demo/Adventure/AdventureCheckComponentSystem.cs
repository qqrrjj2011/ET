using System;

namespace ET.Server
{
    [FriendOf(typeof(AdventureCheckComponent))]
    public class AdventureCheckComponentDestorySyste : DestroySystem<AdventureCheckComponent>
    {
        protected override void Destroy(AdventureCheckComponent self)
        {
            foreach (var monsterId in self.CacheEnemyIdList)
            {
                self.Root().GetComponent<UnitComponent>().Remove(monsterId);
            }
            self.CacheEnemyIdList.Clear();
            self.EnemyIdList.Clear();
            self.AniamtionTotalTime = 0;
            self.Random = null;
        }
    }

    [FriendOf(typeof(Unit))]
    [FriendOf(typeof(AdventureCheckComponent))]
    public static class AdventureCheckComponentSystem
    {
        public static bool CheckBattleWinResult(this AdventureCheckComponent self,int battleRound)
        {
            try
            {
                self.ResetAdventureInfo();
                self.SetBattleRandomSeed();
                self.CreateBattleMonsterUnit();
            
                //模拟对战
                bool isSimulationNormal = self.SimulationBattle(battleRound);
                if (!isSimulationNormal)
                {
                    Log.Error("模拟对战失败");
                    return false;
                }

                if (!self.GetParent<Unit>().IsAlive())
                {
                    Log.Error("玩家未存活");
                    return false;
                }
                
                //判定所有怪物是否被击杀
                if (self.GetFirstAliveEnemy() != null)
                {
                    Log.Error("还有怪物存活");
                    return false;
                }
            
                //判定战斗动画时间是否正常
                NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
                long playAnimationTime = TimeInfo.Instance.ServerNow() - numericComponent.GetAsLong(NumericType.AdventureStartTime);
                if ( playAnimationTime < self.AniamtionTotalTime )
                {
                    Log.Error("动画时间不足");
                    return false;
                }
                return true;
            }
            finally
            {
                self.ResetAdventureInfo();
            }
        }
        
        /// <summary>
        /// 设置战斗随机数
        /// </summary>
        /// <param name="self"></param>
        public static void SetBattleRandomSeed(this AdventureCheckComponent self)
        { 
            int seed = self.GetParent<Unit>().GetComponent<NumericComponent>().GetAsInt(NumericType.BattleRandomSeed);
            if (self.Random == null)
            {
                self.Random = new SRandom((uint)seed);
            }
            else
            {
                self.Random.SetRandomSeed((uint)seed);
            }
        }

        /// <summary>
        /// 创建关卡怪物Unit
        /// </summary>
        /// <param name="self"></param>
        /// <param name="levelId"></param>
        public static void CreateBattleMonsterUnit(this AdventureCheckComponent self)
        {
            int levelId = self.GetParent<Unit>().GetComponent<NumericComponent>().GetAsInt(NumericType.AdventureState);
            //生成最大怪物数量
            BattleLevelConfig battleLevelConfig = BattleLevelConfigCategory.Instance.Get(levelId);
            int monsterCount = battleLevelConfig.MonsterIds.Length - self.CacheEnemyIdList.Count;
            for (int i = 0; i < monsterCount ; i++)
            {
                Unit monsterUnit = UnitFactory.CreateMonster(self.Root(),1002);
                self.CacheEnemyIdList.Add(monsterUnit.Id);
            }
            
            //复用怪物Unit
            self.EnemyIdList.Clear();
            for (int i = 0; i < battleLevelConfig.MonsterIds.Length; i++)
            {
                Unit monsterUnit      = self.Root().GetComponent<UnitComponent>().Get(self.CacheEnemyIdList[i]);
                UnitConfig unitConfig = UnitConfigCategory.Instance.Get(battleLevelConfig.MonsterIds[i]);
                monsterUnit.ConfigId  = unitConfig.Id;
                
                NumericComponent numericComponent = monsterUnit.GetComponent<NumericComponent>();
                numericComponent.SetNoEvent(NumericType.MaxHp,monsterUnit.Config().MaxHP);
                numericComponent.SetNoEvent(NumericType.Hp,monsterUnit.Config().MaxHP);
                numericComponent.SetNoEvent(NumericType.DamageValue,monsterUnit.Config().DamageValue);
                numericComponent.SetNoEvent(NumericType.IsAlive,1);
                self.EnemyIdList.Add(monsterUnit.Id);
            }
        }
        
        /// <summary>
        /// 重置冒险关卡信息
        /// </summary>
        /// <param name="self"></param>
        public static void ResetAdventureInfo(this AdventureCheckComponent self)
        {
            self.AniamtionTotalTime = 0;
            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            numericComponent.SetNoEvent(NumericType.Hp,numericComponent.GetAsInt(NumericType.MaxHp));
            numericComponent.SetNoEvent(NumericType.IsAlive,1);
        }

        /// <summary>
        /// 模拟对战
        /// </summary>
        /// <param name="self"></param>
        /// <param name="levelId"></param>
        /// <param name="battleRound"></param>
        /// <returns></returns>
        public static bool SimulationBattle(this AdventureCheckComponent self,int battleRound)
        {
            //开始模拟对战
            for (int i = 0; i < battleRound; ++i)
            {
                if ( i % 2 == 0)
                {
                    //玩家回合
                    Unit monsterUnit = self.GetFirstAliveEnemy();
                    if (monsterUnit == null)
                    {
                        Log.Debug("获取到怪物为空");
                        return false;
                    }
                    self.AniamtionTotalTime += 1000;
                    self.CalcuateDamageHpValue(self.GetParent<Unit>(),monsterUnit);
                }
                else
                {
                    //敌人回合
                    if (!self.GetParent<Unit>().IsAlive())
                    {
                        return false;
                    }
                    
                    for (int j = 0; j < self.EnemyIdList.Count; j++)
                    {
                        Unit monsterUnit = self.Root().GetComponent<UnitComponent>().Get(self.EnemyIdList[j]);
                        if (  !monsterUnit.IsAlive() )
                        {
                            continue;
                        }
                        self.AniamtionTotalTime += 1000;
                        
                        self.CalcuateDamageHpValue(monsterUnit,self.GetParent<Unit>());
                    }
                }
            }
            return true;
        }
        
        /// <summary>
        /// 获取存活的怪物
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Unit GetFirstAliveEnemy(this AdventureCheckComponent self)
        {
            for (int i = 0; i < self.EnemyIdList.Count; i++)
            {
                Unit monsterUnit = self.Root().GetComponent<UnitComponent>().Get(self.EnemyIdList[i]);
                if (monsterUnit.IsAlive())
                {
                    return monsterUnit;
                }
            }
            return null;
        }

        /// <summary>
        /// 计算伤害值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="attackUnit"></param>
        /// <param name="targeUnit"></param>
        public static void CalcuateDamageHpValue(this AdventureCheckComponent self, Unit attackUnit, Unit targeUnit)
        {
            int Hp = targeUnit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp);
            Hp = Hp - DamageCalcuateHelper.CalcuateDamageValue(attackUnit, targeUnit, ref self.Random);
            if (Hp <= 0)
            {
                Hp = 0;
                targeUnit.GetComponent<NumericComponent>().SetNoEvent(NumericType.IsAlive,0);
            }
            targeUnit.GetComponent<NumericComponent>().SetNoEvent(NumericType.Hp,Hp);
        }
        
        
    }
}