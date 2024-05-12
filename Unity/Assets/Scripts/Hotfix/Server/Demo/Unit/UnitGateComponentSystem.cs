namespace ET.Server
{
    [EntitySystemOf(typeof(UnitGateComponent))]
    public static partial class UnitGateComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitGateComponent self, ActorId a)
        {
            self.GateSessionActorId = a;
        }
      
        
    }
}