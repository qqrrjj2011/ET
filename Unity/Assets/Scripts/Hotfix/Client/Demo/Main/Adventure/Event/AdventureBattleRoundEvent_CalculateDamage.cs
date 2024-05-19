using System;
using ET.Client;

namespace ET.Event
{
    [FriendOf(typeof(AdventureComponent))]
    [Event(SceneType.Demo)]
    public class AdventureBattleRoundEvent_CalculateDamage: AEvent<Scene, EventType.AdventureBattleRound>
    {
        protected override async ETTask Run(Scene scene, EventType.AdventureBattleRound args)
        {
            if (!args.AttackUnit.IsAlive() || !args.TargetUnit.IsAlive())
            {
                return;
            }

            SRandom random = args.scene.CurrentScene().GetComponent<AdventureComponent>().Random;
            
            int damage = DamageCalcuateHelper.CalcuateDamageValue(args.AttackUnit, args.TargetUnit, ref random);
            int HP     = args.TargetUnit.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp) - damage;

            if (HP <= 0)
            {
                HP = 0;
                args.TargetUnit.SetAlive(false);
            }
            
            args.TargetUnit.GetComponent<NumericComponent>().Set(NumericType.Hp,HP);
            Log.Debug($"************* {args.TargetUnit.Type() }被攻击剩余血量: {HP} ***************");
            
            EventSystem.Instance.PublishAsync(args.AttackUnit.Root(),new EventType.ShowDamageValueView()
            {
                ZoneScene = args.scene,TargetUnit = args.TargetUnit,DamamgeValue = damage
            }).Coroutine();
            

            await ETTask.CompletedTask;
        }
    }
}