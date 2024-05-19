using Unity.Mathematics;

namespace ET.Client
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene currentScene, UnitInfo unitInfo)
        {
	        UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
	        Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
	        unitComponent.Add(unit);
	        
	        unit.Position = unitInfo.Position;
	        unit.Forward = unitInfo.Forward;
	        
	        NumericComponent numericComponent = unit.AddComponent<NumericComponent>();

			foreach (var kv in unitInfo.KV)
			{
				numericComponent.Set(kv.Key, kv.Value);
			}
	        
	        unit.AddComponent<MoveComponent>();
	        if (unitInfo.MoveInfo != null)
	        {
		        if (unitInfo.MoveInfo.Points.Count > 0)
				{
					unitInfo.MoveInfo.Points[0] = unit.Position;
					unit.MoveToAsync(unitInfo.MoveInfo.Points).Coroutine();
				}
	        }

	        unit.AddComponent<ObjectWait>();

	        unit.AddComponent<XunLuoPathComponent>();
	        
	        EventSystem.Instance.Publish(unit.Scene(), new EventType.AfterUnitCreate() {Unit = unit});
            return unit;
        }
        
        
        public static Unit CreatePlayer(Scene currentScene, UnitInfo unitInfo)
        {
	        UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
	        Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, unitInfo.ConfigId);
	        unitComponent.Add(unit);
	        
	        NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
	        foreach (var kv in unitInfo.KV)
	        {
		        numericComponent.Set(kv.Key, kv.Value);
	        }
	        
	        unit.AddComponent<ObjectWait>();
	        
	        EventSystem.Instance.PublishAsync(currentScene,new ET.EventType.AfterUnitCreate() {Unit = unit}).Coroutine();
	        return unit;
        }
        
        public static async ETTask<Unit> CreateMonster(Scene currentScene, int configId)
        {
	        UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
	        Unit unit = unitComponent.AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), configId);
	        unitComponent.Add(unit);
	        
	        NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
	        
	        numericComponent.SetNoEvent(NumericType.IsAlive,1); 
	        //numericComponent.SetNoEvent(NumericType.DamageValue,unit.Config.DamageValue);
	        numericComponent.SetNoEvent(NumericType.MaxHp,unit.Config().MaxHP);
	        numericComponent.SetNoEvent(NumericType.Hp,unit.Config().MaxHP);
	        
	        unit.AddComponent<ObjectWait>();
 
	        await EventSystem.Instance.PublishAsync(currentScene, new ET.EventType.AfterUnitCreate() {Unit = unit});
	        return unit;
        }
    }
}
