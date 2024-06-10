using System;
using Unity.Mathematics;
using Random = System.Random;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene scene, long id, UnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case UnitType.Player:
                {
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    unit.AddComponent<MoveComponent>();
                    unit.Position = new float3(-10, 0, -10);
			
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    numericComponent.Set(NumericType.Speed, 6f); // 速度是6米每秒
                    numericComponent.Set(NumericType.AOI, 15000); // 视野15米
                    
                    foreach (var config in PlayerNumericConfigCategory.Instance.GetAll())
                    {
                        if ( config.Value.BaseValue == 0 )
                        {
                            continue;
                        }

                        if ( config.Key < 3000 ) //小于3000的值都用加成属性推导
                        {
                            int baseKey = config.Key * 10 + 1;
                            numericComponent.SetNoEvent(baseKey,config.Value.BaseValue);
                        }
                        else
                        {
                            //大于3000的值 直接使用
                            numericComponent.SetNoEvent(config.Key,config.Value.BaseValue);
                        }
                    }
                    
                    unit.AddComponent<ServerBagComponent>();
                    unit.AddComponent<EquipmentsComponent>();
                    unit.AddComponent<ForgeComponent>();
                    unit.AddComponent<ServerTasksComponent>();
                    unit.AddComponent<UnitRoleInfo>();
                    unit.AddComponent<GeneralsComponent>();
                    
                    unitComponent.Add(unit);
                    // 加入aoi
                    //111 unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }

        /// <summary>
        /// 创建将领
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static Unit CreateGenerals(Scene scene, long Id, int configId)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(Id,configId);
            unit.AddComponent<PathfindingComponent,string>(scene.Name);
            unit.AddComponent<MoveComponent>();
            unit.Position = new float3(-10+new Random().Next(-5,5), 0, -10+new Random().Next(-5,5));

            NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
            numericComponent.SetNoEvent(NumericType.MaxHp, unit.Config().MaxHP);
            numericComponent.SetNoEvent(NumericType.Hp, unit.Config().MaxHP);
            numericComponent.SetNoEvent(NumericType.DamageValue, unit.Config().DamageValue);
            numericComponent.SetNoEvent(NumericType.IsAlive, 1); 
            numericComponent.Set(NumericType.Speed, 6f); // 速度是6米每秒
            numericComponent.Set(NumericType.AOI, 15000); // 视野15米
            return unit;
        }

        public static Unit CreateMonster(Scene scene, int configId)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), configId);
            NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
            
            numericComponent.SetNoEvent(NumericType.MaxHp,unit.Config().MaxHP);
            numericComponent.SetNoEvent(NumericType.Hp,unit.Config().MaxHP);
            numericComponent.SetNoEvent(NumericType.DamageValue,unit.Config().DamageValue);
            numericComponent.SetNoEvent(NumericType.IsAlive,1);
            
            unitComponent.Add(unit);
            return unit;
        }
    }
}