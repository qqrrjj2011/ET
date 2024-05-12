namespace ET.Server
{
	[ComponentOf(typeof(Unit))]
	//public class UnitGateComponent : Entity, IAwake<ActorId>, ITransfer
	public class UnitGateComponent : Entity, IAwake<ActorId>    // ActorId类型传送有问题，新版本暂时用不到UnitGateComponent的传送，先注释
	{
		public ActorId GateSessionActorId { get; set; }
	}
}